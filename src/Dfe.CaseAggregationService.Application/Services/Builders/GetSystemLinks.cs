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

        string GetRecastTitleLink(params string[] formatKeys);

        string GetMfspTitleLink(params string[] formatKeys);
    }

    public class GetSystemLinks: IGetSystemLinks
    {
        public string GetPrepareTitleLink(params string[] formatKeys)
        {
            return string.Format("https://dev.prepare-conversions.education.gov.uk/task-list/{0}", formatKeys) ;
        }

        public string GetRecastTitleLink(params string[] formatKeys)
        {
            return string.Format("https://dev.record-concerns-support-trusts.education.gov.uk/case/{0}/management", formatKeys);
        }

        public string GetMfspTitleLink(params string[] formatKeys)
        {
            return string.Format("https://dev.manage-free-school-projects.education.gov.uk/projects/{0}/overview", formatKeys);
        }
    }
}
