using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class IntegrationWrapper<T>(IGetCaseInfo<T> mapper, ILogger logger)
    {
        protected IEnumerable<UserCaseInfo> ProcessResult(Task<IEnumerable<T>> cases)
        {
            if (!cases.IsFaulted)
            {
                var recast = cases.Result.ToList();
                return recast.Select(mapper.GetCaseInfo);
            }

            if (cases.Exception is not null)
                logger.LogError(cases.Exception, cases.Exception.Message);

            return [];
        }
    }
}
