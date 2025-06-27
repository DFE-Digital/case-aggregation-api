using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Dfe.CaseAggregationService.Infrastructure.Security.Authorization;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAcademisationRepository, AcademisationApiClient>();

            //Cache service
            services.AddServiceCaching(config);
            
            // Authentication
            services.AddCustomAuthorization(config);

            return services;
        }
    }
}
