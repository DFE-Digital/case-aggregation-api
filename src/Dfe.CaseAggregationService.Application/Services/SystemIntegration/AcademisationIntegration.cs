using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class AcademisationIntegration
        (IAcademisationRepository repo,
        IGetCaseInfo<AcademisationSummary> mapper,
        ILogger<AcademisationIntegration> logger)
        : ISystemIntegration
    {
        public Task<IEnumerable<UserCaseInfo>> GetCasesForQuery(GetCasesForUserQuery query, CancellationToken cancellationToken)
        {
            if (query.IncludePrepare)
            {
                bool includeConversions = query.FilterProjectTypes?.Contains("Conversion") ?? false;
                bool includeTransfers = query.FilterProjectTypes?.Contains("Transfer") ?? false;
                bool includeFormAMat = query.FilterProjectTypes?.Contains("Form a MAT") ?? false;

                return repo.GetAcademisationSummaries(query.UserEmail, includeConversions, includeTransfers, includeFormAMat, query.SearchTerm)
                    .ContinueWith(ProcessAcademisation, cancellationToken);
            }
            
            return Task.FromResult<IEnumerable<UserCaseInfo>>(new List<UserCaseInfo>());
        }

        private IEnumerable<UserCaseInfo> ProcessAcademisation(Task<IEnumerable<AcademisationSummary>> cases)
        {
            if (!cases.IsFaulted)
            {
                var academisation = cases.Result.ToList();
                return academisation.Select(mapper.GetCaseInfo);
            }

            if(cases.Exception is not null)
                logger.LogError(cases.Exception, cases.Exception.Message);

            return [];
        }
    }
}
