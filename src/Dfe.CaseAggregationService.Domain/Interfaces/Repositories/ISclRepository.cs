using Dfe.CaseAggregationService.Domain.Common;

namespace Dfe.CaseAggregationService.Domain.Interfaces.Repositories
{
    public interface ISclRepository<TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
    }
}
