using System.Globalization;
using Dfe.AcademiesApi.Client.Contracts;
using Dfe.CaseAggregationService.Domain.Entities.SigChange;
using Dfe.CaseAggregationService.Domain.Interfaces.Repositories;

namespace Dfe.CaseAggregationService.Infrastructure.Gateways
{
    public class SignificantChangeApiClient(ISignificantChangesV4Client significantChangesClient) : ISigChangeRepository
    {
        const string format = "dd/MM/yyyy";

        public async Task<IEnumerable<SigChangeSummary>> GetSigChangeSummaries(string? userName, CancellationToken cancellationToken)
        {

            var response = await significantChangesClient.SearchSignificantChangesAsync(userName, null, null, null, null, cancellationToken);

            if (response.Data == null)
                throw new Exception();

            return response.Data.Where(x => x.AllActionsCompleted != true)
            .Select(x => new SigChangeSummary()
            {
                SigChangeId = x.SigChangeId.ToString() ?? "",
                AcademyName = x.AcademyName,
                ChangeType = x.TypeOfSigChangeMapped,
                Trust = x.TrustName,
                Urn = x.Urn.ToString(),
                LocalAuthority = x.LocalAuthority,
                Region = x.Region,
                DateOfDecision = ParseNullableDate(x.DecisionDate),
                CreatedDate = ParseDate(x.ChangeCreationDate),
                UpdatedDate = ParseDate(x.ChangeEditDate)
            });
        }

        private DateTime? ParseNullableDate(string? dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return null;

            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
                return date;

            return null;
        }

        private DateTime ParseDate(string? dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return DateTime.MinValue;


            return DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture,
                 DateTimeStyles.None, out var date) ? date : DateTime.MinValue;
        }

    }
}
