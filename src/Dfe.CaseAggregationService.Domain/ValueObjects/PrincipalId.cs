using Dfe.CaseAggregationService.Domain.Common;

namespace Dfe.CaseAggregationService.Domain.ValueObjects
{
    public record PrincipalId(int Value) : IStronglyTypedId;
}
