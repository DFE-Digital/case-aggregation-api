using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public interface IGetSystemLinks
    {
        string GetPrepareTitleLink(params string[] formatKeys);
    }

    public class GetSystemLinks: IGetSystemLinks
    {
        public string GetPrepareTitleLink(params string[] formatKeys)
        {
            return string.Format("https://dev.prepare-conversions.education.gov.uk/task-list/{0}", formatKeys) ;
        }
    }
}
