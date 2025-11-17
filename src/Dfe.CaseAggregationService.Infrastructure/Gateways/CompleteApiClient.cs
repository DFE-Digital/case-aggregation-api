using Dfe.CaseAggregationService.Domain.Entities.Complete;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Dfe.Complete.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class CompleteApiClient : ApiClient, ICompleteRepository
    {
        private readonly IProjectsClient _completeProjects;

        public CompleteApiClient(
            IHttpClientFactory clientFactory,
            ILogger<ApiClient> logger,
            IProjectsClient completeProjects,
            string httpClientName = "CompleteApiClient") : base(clientFactory, logger, httpClientName)
        {
            _completeProjects = completeProjects;
        }

        public async Task<IEnumerable<CompleteSummary>> GetCompleteSummaryForUser(string userEmail,
            string[]? requestFilterProjectTypes, CancellationToken cancellationToken)
        {
            var projects = await _completeProjects.ListAllProjectsForUserAsync(ProjectState.Active,
                null,
                ProjectUserFilter.AssignedTo,
                null,
                null,
                userEmail,
                0,
                100,
                cancellationToken);

            var output = projects.Select(x => new CompleteSummary(x.ProjectId.Value,
                GetProjectType(x.ProjectType, x.IsFormAMat),
                x.SchoolOrAcademyName ?? "",
                x.Urn?.ToString() ?? "",
                x.ConversionOrTransferDate,
                x.IncomingTrustName ?? "",
                x.OutgoingTrustName ?? "",
                x.LocalAuthority ?? "",
                x.CreatedDate,
                x.UpdatedDate))
                .Where(x => FilterProjectTypes(x.CaseType, requestFilterProjectTypes));

            return output;
        }

        private static CompleteProjectType GetProjectType(ProjectType? projectType, bool? formAMat)
        {
            if (projectType == null)
            {
                return CompleteProjectType.Unknown;
            }

            if (formAMat.HasValue && formAMat.Value)
            {
                return projectType switch
                {
                    ProjectType.Conversion => CompleteProjectType.FormAMatConversion,
                    ProjectType.Transfer => CompleteProjectType.FormAMatTransfer,
                    _ => CompleteProjectType.Unknown
                };
            }
            else
            {
                return projectType switch
                {
                    ProjectType.Conversion => CompleteProjectType.Conversion,
                    ProjectType.Transfer => CompleteProjectType.Transfer,
                    _ => CompleteProjectType.Unknown
                };
            }
        }

        private static bool FilterProjectTypes(CompleteProjectType projectType, string[]? filters)
        {
            if (filters is not { Length: > 0 })
            {
                return true;
            }

            var conversion = filters.Contains("Conversion");
            var transfer = filters.Contains("Transfer");
            var formAMat = filters.Contains("Form a MAT");

            switch (projectType)
            {
                case CompleteProjectType.Conversion:
                    return conversion;
                case CompleteProjectType.Transfer:
                    return transfer;
                case CompleteProjectType.FormAMatConversion:
                    return (conversion || formAMat) && !transfer;
                case CompleteProjectType.FormAMatTransfer:
                    return (transfer || formAMat) && !conversion;
            }

            return filters.Contains(projectType.ToString(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
