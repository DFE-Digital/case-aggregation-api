using System.Diagnostics.CodeAnalysis;
using Dfe.CaseAggregationService.Domain.Common;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.CaseAggregationService.Infrastructure.Database;

namespace Dfe.CaseAggregationService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SclRepository<TAggregate>(SclContext dbContext)
        : Repository<TAggregate, SclContext>(dbContext), ISclRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
    }
}