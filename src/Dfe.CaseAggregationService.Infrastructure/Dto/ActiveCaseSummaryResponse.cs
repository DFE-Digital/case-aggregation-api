using System.ComponentModel;

namespace Dfe.CaseAggregationService.Domain.Entities.Recast
{

    public class ApiResponseV2<TResponse> where TResponse : class
    {
        public IEnumerable<TResponse> Data { get; set; }
        public PagingResponse Paging { get; set; }

        public ApiResponseV2() => Data = new List<TResponse>();

        public ApiResponseV2(IEnumerable<TResponse> data, PagingResponse pagingResponse)
        {
            Data = data;
            Paging = pagingResponse;
        }

        public ApiResponseV2(TResponse data) => Data = new List<TResponse> { data };
    }

    public class PagingResponse
    {
        /// <summary>
        /// The current page we are on
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// This is the total record count
        /// </summary>
        public int RecordCount { get; set; }
        public string NextPageUrl { get; set; }
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        /// <summary>
        /// The total number of pages
        /// This is calculated by the total records divided by the records per page
        /// </summary>
        public int TotalPages { get; set; }
    }

    public abstract record CaseSummaryResponse
    {
        public long CaseUrn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CaseLastUpdatedAt { get; set; }
        public string StatusName { get; set; }
        public string TrustUkPrn { get; set; }
        public IEnumerable<ActionOrDecision> Decisions { get; set; }
        public IEnumerable<ActionOrDecision> FinancialPlanCases { get; set; }
        public IEnumerable<ActionOrDecision> NoticesToImprove { get; set; }
        public IEnumerable<ActionOrDecision> NtiWarningLetters { get; set; }
        public IEnumerable<ActionOrDecision> NtisUnderConsideration { get; set; }
        public IEnumerable<ActionOrDecision> SrmaCases { get; set; }
        public IEnumerable<ActionOrDecision> TrustFinancialForecasts { get; set; }
        public IEnumerable<ActionOrDecision> TargetedTrustEngagements { get; set; }

        public Division? Division { get; set; }

        public string? Area { get; set; }

        public record ActionOrDecision(DateTime CreatedAt, DateTime? ClosedAt, string Name);
        public record Concern(string Name, ConcernsRatingResponse Rating, DateTime CreatedAt);
    }

    public record ActiveCaseSummaryResponse : CaseSummaryResponse
    {
        public IEnumerable<Concern> ActiveConcerns { get; set; }
        public ConcernsRatingResponse Rating { get; set; }
    }

    public enum Division
    {
        [Description("SFSO")]
        SFSO = 1,
        [Description("Regions Group")]
        RegionsGroup = 2,
    }

    public class ConcernsRatingResponse
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Id { get; set; }
    }

}
