using Dfe.CaseAggregationService.Application.Common.Behaviours;
using Dfe.CaseAggregationService.Application.MappingProfiles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDependencyGroup(
            this IServiceCollection services, IConfiguration config)
        {
            var performanceLoggingEnabled = config.GetValue<bool>("Features:PerformanceLoggingEnabled");

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

                if (performanceLoggingEnabled)
                {
                    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
                }
            });
            
            services.AddAutoMapper(typeof(SchoolProfile));
            services.AddAutoMapper(typeof(ConversionInfoProfile));
            services.AddAutoMapper(typeof(TransferInfoProfile));

            services.AddScoped<IGetCaseInfo<AcademisationSummary>, GetCaseInfoFromAcademisationSummary>();
            services.AddScoped<IGetGuidanceLinks, GetGuidanceLinks>();
            services.AddScoped<IGetResourcesLinks, GetResourcesLinks>();

            services.AddBackgroundService();

            return services;
        }
    }
}
