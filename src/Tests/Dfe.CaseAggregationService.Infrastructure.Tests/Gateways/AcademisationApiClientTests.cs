using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Infrastructure.Tests.Gateways
{
    public class AcademisationApiClientTests
    {
        private readonly AcademisationApiClient _repo;

        public AcademisationApiClientTests()
        {
            var httpClient = Substitute.For<IHttpClientFactory>();
            var logger = Substitute.For<ILogger<AcademisationApiClient>>();

            _repo = new AcademisationApiClient(httpClient, logger);

        }
    }
}
