using AutoFixture;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.SystemIntegration;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.Services.SystemIntegration
{
    public class MfspIntegrationTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task GetCasesForQuery_WhenIncludeManageFreeSchools_ReturnsMappedCases()
        {
            var integration = CreateIntegration();
            var query = new GetCasesForUserQuery(
                "test user",
                "test.user@education.gov.uk",
                false,
                false,
                false,
                true,
                false,
                false,
                []);

            var result = (await integration.GetCasesForQuery(query, CancellationToken.None)).ToArray();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetCasesForQuery_WhenNotIncluded_ReturnsEmpty()
        {
            var integration = CreateIntegration();
            var query = new GetCasesForUserQuery(
                "test user",
                "test.user@education.gov.uk",
                false,
                false,
                false,
                false,
                false,
                false,
                []);

            var result = (await integration.GetCasesForQuery(query, CancellationToken.None)).ToArray();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetCasesForQuery_WhenRepositoryFails_ReturnsEmpty()
        {
            var integration = CreateIntegration(repoFaulted: true);
            var query = new GetCasesForUserQuery(
                "test user",
                "test.user@education.gov.uk",
                false,
                false,
                false,
                true,
                false,
                false,
                []);

            var result = (await integration.GetCasesForQuery(query, CancellationToken.None)).ToArray();

            result.Should().BeEmpty();
        }

        private MfspIntegration CreateIntegration(bool repoFaulted = false)
        {
            var repo = Substitute.For<IMfspRepository>();
            var mapper = Substitute.For<IGetCaseInfo<MfspSummary>>();
            var logger = Substitute.For<ILogger<MfspIntegration>>();

            if (repoFaulted)
            {
                repo.GetMfspSummaries(Arg.Any<string>(), Arg.Any<string[]?>())
                    .Returns(Task.FromException<IEnumerable<MfspSummary>>(
                        new HttpRequestException("MFSP unavailable")));
            }
            else
            {
                repo.GetMfspSummaries(Arg.Any<string>(), Arg.Any<string[]?>())
                    .Returns([_fixture.Create<MfspSummary>()]);
            }

            mapper.GetCaseInfo(Arg.Any<MfspSummary>()).Returns(_fixture.Create<UserCaseInfo>());

            return new MfspIntegration(repo, mapper, logger);
        }
    }
}
