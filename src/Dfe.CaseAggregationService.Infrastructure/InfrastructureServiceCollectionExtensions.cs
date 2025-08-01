using Dfe.AcademiesApi.Client.Contracts;
using Dfe.AcademiesApi.Client;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Dfe.CaseAggregationService.Infrastructure.Security.Authorization;
using Dfe.Complete.Api.Client.Extensions;
using Dfe.Complete.Client;
using Dfe.Complete.Client.Contracts;
using Dfe.TramsDataApi.Client.Extensions;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IAcademisationRepository, AcademisationApiClient>();
            services.AddScoped<IRecastRepository, RecastApiClient>();
            services.AddScoped<IMfspRepository, MfspApiClient>();
            services.AddScoped<ICompleteRepository, CompleteApiClient>();

            services.AddAcademiesApiClient<ITrustsV4Client, TrustsV4Client>(config);

            services.AddCompleteApiClient<IProjectsClient, ProjectsClient>(config);
            //Cache service
            services.AddServiceCaching(config);
            
            // Authentication
            services.AddCustomAuthorization(config);

            return services;
        }
    }
}
