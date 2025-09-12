using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Security.Policy;

namespace Dfe.CaseAggregationService.Infrastructure.Tests.Gateways
{
    public class AcademisationApiClientTests
    {
        private readonly AcademisationApiClient _repo;

        public AcademisationApiClientTests()
        {
            var httpClient = Substitute.For<IHttpClientFactory>();
            var logger = Substitute.For<ILogger<ApiClient>>();

            _repo = new AcademisationApiClient(httpClient, logger);

        }
    }
}
