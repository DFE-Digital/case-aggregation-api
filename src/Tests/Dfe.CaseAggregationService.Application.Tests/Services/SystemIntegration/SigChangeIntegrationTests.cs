using AutoFixture;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.SystemIntegration;
using Dfe.CaseAggregationService.Domain.Entities.SigChange;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.Services.SystemIntegration
{
    public class SigChangeIntegrationTests
    {
        private readonly Fixture _fixture;

        public SigChangeIntegrationTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_GetCaseForUserReturnsSingleItem()
        {
            var integration = CreateSigChangeIntegration();

            var query = new GetCasesForUserQuery(UserName: "test user", UserEmail: "test.user@education.gov.uk", true,
                false, false, false, false, false, []);
            
            var resultEnumerable = await integration.GetCasesForQuery(query, CancellationToken.None);
            var result = resultEnumerable.ToArray() ?? [];
            result.Should().NotBeNullOrEmpty();

            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task Handle_GetCaseForUserIfNotSelectedReturnsEmpty()
        {
            var integration = CreateSigChangeIntegration();

            var query = new GetCasesForUserQuery(UserName: "test user", UserEmail: "test.user@education.gov.uk", false,
                false, false, false, false, false, []);

            var resultEnumerable = await integration.GetCasesForQuery(query, CancellationToken.None);
            var result = resultEnumerable.ToArray() ?? [];
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_GetCaseForUserIfProjectTypeFilterReturnsEmpty()
        {
            var integration = CreateSigChangeIntegration();

            var query = new GetCasesForUserQuery(UserName: "test user", UserEmail: "test.user@education.gov.uk", true,
                false, false, false, false, false, ["project type"]);

            var resultEnumerable = await integration.GetCasesForQuery(query, CancellationToken.None);
            var result = resultEnumerable.ToArray() ?? [];
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        private SigChangeIntegration CreateSigChangeIntegration()
        {
            ISigChangeRepository repo = Substitute.For<ISigChangeRepository>();
            IGetCaseInfo<SigChangeSummary> mapper = Substitute.For<IGetCaseInfo<SigChangeSummary>>();
            ILogger<SigChangeIntegration> logger = Substitute.For<ILogger<SigChangeIntegration>>();

            repo.GetSigChangeSummaries(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns([_fixture.Create<SigChangeSummary>()
            ]);
            mapper.GetCaseInfo(Arg.Any<SigChangeSummary>()).Returns(_fixture.Create<UserCaseInfo>());

            var integration = new SigChangeIntegration(repo, mapper, logger);
            return integration;
        }
    }
}
