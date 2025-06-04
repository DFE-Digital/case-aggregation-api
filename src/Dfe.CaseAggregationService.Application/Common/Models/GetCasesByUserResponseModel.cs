using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Application.Common.Models
{
    public class GetCasesByUserResponseModel
    {
        public int TotalRecordCount { get; set; }

        public List<UserCaseInfo> CaseInfos { get; set; }
    }
}
