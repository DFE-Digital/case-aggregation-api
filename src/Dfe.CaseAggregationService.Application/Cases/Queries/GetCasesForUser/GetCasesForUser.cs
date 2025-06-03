using System.ComponentModel;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using MediatR;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser
{
    public enum SortCriteria
    {
        [Description("CreatedDateDescending")]
        CreatedDateDescending = 1,
        [Description("CreatedDateAscending")]
        CreatedDateAscending = 2,
        [Description("UpdatedDateAscending")]
        UpdatedDateAscending = 3,
        [Description("UpdatedDateDescending")]
        UpdatedDateDescending = 4,
    }

    public record GetCasesForUserQuery(
        string UserName,
        string UserEmail,
        bool IncludeSignificantChange,
        bool IncludePrepare,
        bool IncludeComplete,
        bool IncludeManageFreeSchools,
        bool IncludeConcerns,
        bool IncludeWarningNotices,
        string[] FilterProjectTypes,
        string? searchTerm = null,
        SortCriteria? SortCriteria = SortCriteria.CreatedDateDescending) : IRequest<Result<List<UserCaseInfo>>>;

    public class GetCasesForUserQueryHandler(
        IGetAcademisationSummary getAcademisationSummary,
        IGetCaseInfo<AcademisationSummary> academisationMap,
        ILogger<GetCasesForUserQueryHandler> logger)
        : IRequestHandler<GetCasesForUserQuery, Result<List<UserCaseInfo>>>
    {
        public async Task<Result<List<UserCaseInfo>>> Handle(GetCasesForUserQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Gathering cases for user");

            var userCaseInfo = new List<UserCaseInfo>();

            var listOfTasks = new List<Task<IEnumerable<UserCaseInfo>>>();

            if (request.IncludePrepare)
            {
                bool includeConversions = request.FilterProjectTypes.Contains("Conversion");
                bool includeTransfers = request.FilterProjectTypes.Contains("Transfer");
                bool includeFormAMat = request.FilterProjectTypes.Contains("Form a MAT");

                listOfTasks.Add(getAcademisationSummary.GetAcademisationSummaries(request.UserEmail, includeConversions, includeTransfers, includeFormAMat, request.searchTerm)
                    .ContinueWith(ProcessAcademisation, cancellationToken));
            }

            try
            {
                await Task.WhenAll(listOfTasks.ToArray());
            }
            catch
            {
                logger.LogWarning("A call has failed when aggregating cases");
            }

            listOfTasks.ForEach(x => userCaseInfo.AddRange(x.Result));

            SortOutput(request, userCaseInfo);


            return Result<List<UserCaseInfo>>.Success(userCaseInfo);

        }

        private static void SortOutput(GetCasesForUserQuery request, List<UserCaseInfo> userCaseInfo)
        {
            if(request.SortCriteria == SortCriteria.CreatedDateAscending)
                userCaseInfo.Sort((x1, x2) => DateTime.Compare(x1.CreatedDate, x2.CreatedDate));
            if (request.SortCriteria == SortCriteria.CreatedDateDescending)
                userCaseInfo.Sort((x1, x2) => DateTime.Compare(x2.CreatedDate, x1.CreatedDate));
            if (request.SortCriteria == SortCriteria.UpdatedDateAscending)
                userCaseInfo.Sort((x1, x2) => DateTime.Compare(x1.UpdatedDate, x2.UpdatedDate));
            if (request.SortCriteria == SortCriteria.UpdatedDateDescending)
                userCaseInfo.Sort((x1, x2) => DateTime.Compare(x2.UpdatedDate, x1.UpdatedDate));
        }

        private IEnumerable<UserCaseInfo> ProcessAcademisation(Task<IEnumerable<AcademisationSummary>> cases)
        {
            if (!cases.IsFaulted)
            {
                var academisation = cases.Result.ToList();
                return academisation.Select(academisationMap.GetCaseInfo);
            }

            return [];
        }
    }
}
