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

        public async Task<IEnumerable<CompleteSummary>> GetCompleteSummaryForUser(string userEmail, CancellationToken cancellationToken)
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
                x.CompletionDate,
                x.IncomingTrustName ?? "",
                x.OutgoingTrustName ?? "",
                x.LocalAuthority ?? "",
                x.CreatedDate,
                x.UpdatedDate));
            
            return output;
        }

        private static string GetProjectType(ProjectType? projectType, bool? formAMat)
        {
            if (projectType == null)
            {
                return "";
            }

            return (formAMat.HasValue && formAMat.Value ? "Form a MAT " : "") 
                   + projectType switch
                   {
                       ProjectType.Conversion => "Conversion",
                       ProjectType.Transfer => "Transfer",
                       _ => ""
                   };
        }
    }
}
