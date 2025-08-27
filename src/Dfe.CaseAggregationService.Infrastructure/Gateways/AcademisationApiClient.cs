using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class AcademisationApiClient : ApiClient, IAcademisationRepository
    {
        public AcademisationApiClient(
            IHttpClientFactory clientFactory, 
            ILogger<ApiClient> logger,
            string httpClientName = "AcademisationApiClient") : base(clientFactory, logger, httpClientName)
        {
            
        }

        public async Task<IEnumerable<AcademisationSummary>> GetAcademisationSummaries(string userEmail, bool includeConversions, bool includeTransfers, bool includeFormAMat, string? searchTerm)
        {
            const string baseUrl = "summary/projects";

            var queryParams = new Dictionary<string, string?>
            {
                { "email", userEmail }
            };

            if (includeConversions)
            {
                queryParams.Add("includeConversions", "true");
            }

            if (includeTransfers)
            {
                queryParams.Add("includeTransfers", "true");
            }

            if (includeFormAMat)
            {
                queryParams.Add("includeFormAMat", "true");
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryParams.Add("searchTerm", searchTerm);
            }

            var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

            var result = await Get<IEnumerable<AcademisationSummary>>(url);

            var output = new List<AcademisationSummary>();

            foreach (var item in result)
            {
                if (await FilterProjectStatus(item))
                {
                    output.Add(item);
                }
            }

            return output;
        }

        private async Task<bool> FilterProjectStatus(AcademisationSummary summary)
        {
            if(summary.ConversionsSummary is not null)
            {
                return CheckConversion(summary.ConversionsSummary.ProjectStatus);
            }

            if (summary.TransfersSummary is not null)
            {
                return summary.TransfersSummary.Status is null;
            }

            if (summary.FormAMatSummary is not null)
            {

                var result = await Get<dynamic>($"conversion-project/formamatproject/{summary.Id}");

                foreach (var project in result.projects)
                {

                    if (CheckConversion(project.projectStatus) == false)
                        return false;
                }

                return true;

            }

            return false;

            bool CheckConversion(string? status)
            {
                if (status == null)
                    return false;

                if (status.Contains("Pre-AO"))
                {
                    return true;
                }

                return status == "Deferred";
            }
        }
    }

}
