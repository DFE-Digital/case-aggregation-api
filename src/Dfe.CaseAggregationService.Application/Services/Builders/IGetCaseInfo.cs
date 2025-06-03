using Dfe.CaseAggregationService.Application.Common.Models;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{ public interface IGetCaseInfo<in T>
    {
        UserCaseInfo GetCaseInfo(T summary);
    }
}
