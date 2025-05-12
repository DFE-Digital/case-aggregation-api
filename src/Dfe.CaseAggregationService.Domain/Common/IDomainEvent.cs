using MediatR;

namespace Dfe.CaseAggregationService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
