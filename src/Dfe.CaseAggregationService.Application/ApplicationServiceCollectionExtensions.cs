using Dfe.CaseAggregationService.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Entities.Complete;
using Dfe.CaseAggregationService.Domain.Entities.Mfsp;
using Dfe.CaseAggregationService.Domain.Entities.Recast;
using Dfe.CaseAggregationService.Application.Services.SystemIntegration;

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

            services.AddScoped<IGetCaseInfo<AcademisationSummary>, GetCaseInfoFromAcademisationSummary>();
            services.AddScoped<IGetCaseInfo<RecastSummary>, GetCaseInfoFromRecastSummary>();
            services.AddScoped<IGetCaseInfo<MfspSummary>, GetCaseInfoFromMfspSummary>();
            services.AddScoped<IGetCaseInfo<CompleteSummary>, GetCaseInfoFromCompleteSummary>();
            services.AddScoped<ISystemIntegration, AcademisationIntegration>();
            services.AddScoped<ISystemIntegration, MfspIntegration>();
            services.AddScoped<ISystemIntegration, CompleteIntegration>();
            services.AddScoped<ISystemIntegration, RecastIntegration>();

            services.AddScoped<IGetGuidanceLinks, GetGuidanceLinks>();
            services.AddScoped<IGetResourcesLinks, GetResourcesLinks>();
            services.AddScoped<IGetSystemLinks, GetSystemLinks>();

            services.AddBackgroundService();

            return services;
        }
    }
}
