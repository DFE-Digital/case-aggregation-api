using AutoMapper;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using MediatR;
using Dfe.SignificantChange.Client.Contracts;

namespace Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser
{
    public record GetCasesForUserQuery(string UserName, string UserEmail) : IRequest<Result<UserCaseInfo?>>;

    public class GetCasesForUserQueryHandler(
        ICaseClient caseClient,
        IGetAcademisationSummary getAcademisationSummary,
        IMapper mapper)
        : IRequestHandler<GetCasesForUserQuery, Result<UserCaseInfo?>>
    {
        public async Task<Result<UserCaseInfo?>> Handle(GetCasesForUserQuery request, CancellationToken cancellationToken)
        {

            var significantChanges = caseClient.GetSignificantChangeByUserAsync(request.UserName);

            var academisationSummary = getAcademisationSummary.GetAcademisationSummaries(request.UserEmail);

            await Task.WhenAll([academisationSummary]);

            var userCaseInfo = new UserCaseInfo();

            userCaseInfo.SignificantChangeCases = mapper.Map<List<SignificantChangeCaseInfo>>(significantChanges.Result);
            var academiesSummary = academisationSummary.Result.ToList();
            userCaseInfo.ConversionSummaries = academiesSummary.Where(x => x.ConversionsSummary != null).Select(x => mapper.Map<ConversionsCaseInfo>(x)).ToList();
            userCaseInfo.TransfersSummaries = academiesSummary.Where(x => x.TransfersSummary != null).Select(x => mapper.Map<TransfersCaseInfo>(x)).ToList();
            return Result<UserCaseInfo?>.Success(userCaseInfo);

        }
    }
}
