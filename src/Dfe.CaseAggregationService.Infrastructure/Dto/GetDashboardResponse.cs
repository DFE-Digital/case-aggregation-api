using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Infrastructure.Dto
{
    public class PagingResponse
    {
        public int Page { get; set; }

        public int RecordCount { get; set; }
        public string NextPageUrl { get; set; }
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int TotalPages { get; set; }
    }

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
    public class GetProjectSummaryResponse
    {
        public string ProjectTitle { get; set; }

        public string ProjectId { get; set; }

        public string TrustName { get; set; }

        public string Region { get; set; }

        public string LocalAuthority { get; set; }

        public string RealisticOpeningYear { get; set; }

        public string ProjectStatus { get; set; }

        public string ProjectManagedBy { get; set; }

        public string ProjectType { get; set; }

        public string ProjectManagedByEmail { get; set; }

        public string SchoolType { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
