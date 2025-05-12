using Dfe.CaseAggregationService.Domain.Common;
using Dfe.CaseAggregationService.Domain.Entities.Schools;

namespace Dfe.CaseAggregationService.Domain.Events
{
    public class SchoolCreatedEvent(School school) : IDomainEvent
    {
        public School School { get; } = school;

        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
