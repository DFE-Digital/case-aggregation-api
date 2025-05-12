using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using Dfe.CaseAggregationService.Infrastructure.Database;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Dfe.CaseAggregationService.Infrastructure.Repositories;
using Dfe.CaseAggregationService.Infrastructure.Security.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            //Repos
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped(typeof(ISclRepository<>), typeof(SclRepository<>));

            services.AddScoped<IGetAcademisationSummary, AcademisationApiClient>();

            //Cache service
            services.AddServiceCaching(config);

            //Db
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<SclContext>(options =>
                options.UseSqlServer(connectionString));

            // Authentication
            services.AddCustomAuthorization(config);

            return services;
        }
    }
}
