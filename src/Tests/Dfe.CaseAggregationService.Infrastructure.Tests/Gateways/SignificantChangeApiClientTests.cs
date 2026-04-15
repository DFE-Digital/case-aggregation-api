using Dfe.AcademiesApi.Client.Contracts;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Infrastructure.Tests.Gateways
{
    public class SignificantChangeApiClientTests
    {
        private const string DeliveryOfficer = "Delivery Officer";

        private readonly SignificantChangeApiClient _repo;

        public SignificantChangeApiClientTests()
        {
            var httpClient = Substitute.For<IHttpClientFactory>();
            var logger = Substitute.For<ILogger<ApiClient>>();
            var sigChangeProjects = Substitute.For<ISignificantChangesV4Client>();

            var results = new PagedDataResponseOfSignificantChangeDto()
            {
                Data =
                [
                    new()
                    {
                        SigChangeId = 1,
                        AcademyName = "Academy 1",
                        TypeOfSigChangeMapped = "Type 1",
                        TrustName = "Trust 1",
                        Urn = 123456,
                        LocalAuthority = "LA 1",
                        Region = "Region 1",
                        DecisionDate = "10/02/2026",
                        ChangeCreationDate = "2026-02-10T00:00:00Z",
                        ChangeEditDate = "2026-02-10T00:00:00Z"
                    },

                    new()
                    {
                        SigChangeId = 2,
                        AcademyName = "Academy 2",
                        TypeOfSigChangeMapped = "Type 2",
                        TrustName = "Trust 2",
                        Urn = 123457,
                        LocalAuthority = "LA 2",
                        Region = "Region 2",
                        DecisionDate = "10/02/2026",
                        ChangeCreationDate = "2026-02-10T00:00:00Z",
                        ChangeEditDate = "2026-02-10T00:00:00Z"
                    }
                ]
            };

            sigChangeProjects.SearchSignificantChangesAsync(Arg.Is(DeliveryOfficer),
                    null, null, null, null,
                    Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(results));

            _repo = new SignificantChangeApiClient(sigChangeProjects);
        }

        [Fact]
        public async Task GivenCorrectCallReturnSummary()
        {
            var result = await _repo.GetSigChangeSummaries(DeliveryOfficer, CancellationToken.None);

            Assert.Equal(2, result.Count());

            var first = result.First();

            Assert.Equal("Academy 1", first.AcademyName);
            Assert.Equal("Type 1", first.ChangeType);
            Assert.Equal("Trust 1", first.Trust);
            Assert.Equal("123456", first.Urn);
            Assert.Equal("LA 1", first.LocalAuthority);
            Assert.Equal("Region 1", first.Region);
            Assert.Equal(new DateTime(2026, 2, 10), first.DateOfDecision);
            Assert.Equal(new DateTime(2026, 2, 10), first.CreatedDate);
            Assert.Equal(new DateTime(2026, 2, 10), first.UpdatedDate);

            var second = result.Skip(1).First();

            Assert.Equal("Academy 2", second.AcademyName);
            Assert.Equal("Type 2", second.ChangeType);
            Assert.Equal("Trust 2", second.Trust);
            Assert.Equal("123457", second.Urn);
            Assert.Equal("LA 2", second.LocalAuthority);
            Assert.Equal("Region 2", second.Region);
            Assert.Equal(new DateTime(2026, 2, 10), second.DateOfDecision);
            Assert.Equal(new DateTime(2026, 2, 10), second.CreatedDate);
            Assert.Equal(new DateTime(2026, 2, 10), second.UpdatedDate);
        }

    }
}
