using System.Collections.ObjectModel;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Infrastructure.Tests.Gateways
{
    public class CompleteApiClientTests
    {
        private readonly CompleteApiClient _repo;

        public CompleteApiClientTests()
        {
            var httpClient = Substitute.For<IHttpClientFactory>();
            var logger = Substitute.For<ILogger<ApiClient>>();
            var completeProjects = Substitute.For<IProjectsClient>();

            var results = new ObservableCollection<ListAllProjectsForUserQueryResultModel>()
            {
                BuildFixture(ProjectType.Conversion),
                BuildFixture(ProjectType.Transfer),
                BuildFixture(ProjectType.Transfer),
                BuildFixture(ProjectType.Conversion, true),
                BuildFixture(ProjectType.Transfer, true),
                BuildFixture(ProjectType.Transfer, true)
            };

            completeProjects.ListAllProjectsForUserAsync(Arg.Is(ProjectState.Active),
                    null,
                    ProjectUserFilter.AssignedTo,
                    null,
                    null,
                    "userEmail",
                    0,
                    100,
                    Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(results));

            _repo = new CompleteApiClient(httpClient, logger, completeProjects);
        }

        [Fact]
        public async Task GivenCorrectCallReturnSummary()
        {
            var result = await _repo.GetCompleteSummaryForUser("userEmail", null, CancellationToken.None);

            Assert.Equal(6, result.Count());
        }

        [Fact]
        public async Task GivenFilterConversionReturnCorrectResult()
        {
            var result = await _repo.GetCompleteSummaryForUser("userEmail", ["Conversion"], CancellationToken.None);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GivenFilterTransferReturnCorrectResult()
        {
            var result = await _repo.GetCompleteSummaryForUser("userEmail", ["Transfer"], CancellationToken.None);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GivenFilterFormAMatReturnCorrectResult()
        {
            var result = await _repo.GetCompleteSummaryForUser("userEmail", ["Form a MAT"], CancellationToken.None);

            Assert.Equal(3, result.Count());
        }


        private static ListAllProjectsForUserQueryResultModel BuildFixture(ProjectType? projectType = ProjectType.Conversion, bool? isFormAMat = false)
        {
            var result = new ListAllProjectsForUserQueryResultModel
            {
                ProjectId = new ProjectId(),
                ProjectType = projectType,
                IsFormAMat = isFormAMat,
                SchoolOrAcademyName = "",
                Urn = new Urn(),
                CompletionDate = new DateTime(),
                IncomingTrustName = "",
                OutgoingTrustName = "",
                LocalAuthority = "",
                CreatedDate = new DateTime(),
                UpdatedDate = new DateTime()
            };

            return result;
        }
    }
}
