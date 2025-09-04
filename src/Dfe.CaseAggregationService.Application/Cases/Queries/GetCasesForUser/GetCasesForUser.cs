using System.ComponentModel;
using Dfe.CaseAggregationService.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Dfe.CaseAggregationService.Application.Services.SystemIntegration;

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
        string[]? FilterProjectTypes,
        string? SearchTerm = null,
        SortCriteria? SortCriteria = SortCriteria.CreatedDateDescending,
        int Page = 1,
        int RecordCount = 25) : IRequest<Result<GetCasesByUserResponseModel>>;

    public class GetCasesForUserQueryHandler(
        IEnumerable<ISystemIntegration> integrations,
        ILogger<GetCasesForUserQueryHandler> logger)
        : IRequestHandler<GetCasesForUserQuery, Result<GetCasesByUserResponseModel>>
    {

        public async Task<Result<GetCasesByUserResponseModel>> Handle(GetCasesForUserQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Gathering cases for user");

            var userCaseInfo = new List<UserCaseInfo>();

            var listOfTasks = new List<Task<IEnumerable<UserCaseInfo>>>();
            
            try
            {
                listOfTasks.AddRange(integrations.Select(x => x.GetCasesForQuery(request, cancellationToken)));
                await Task.WhenAll(listOfTasks);
            }
            catch (Exception e)
            {
                logger.LogWarning("A call has failed when aggregating cases: {0}", e.Message);
            }

            listOfTasks.ForEach(x => userCaseInfo.AddRange(x.Result));

            var returnModel = new GetCasesByUserResponseModel
            {
                TotalRecordCount = userCaseInfo.Count
            };

            SortOutput(request, userCaseInfo);

            returnModel.CaseInfos = userCaseInfo.Skip((request.Page - 1) * request.RecordCount).Take(request.RecordCount).ToList();

            return Result<GetCasesByUserResponseModel>.Success(returnModel);

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

    }
}
