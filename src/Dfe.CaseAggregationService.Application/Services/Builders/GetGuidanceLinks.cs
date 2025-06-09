using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.CaseAggregationService.Application.Common.Models;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    namespace Dfe.CaseAggregationService.Application.Services.Builders
    {
        public interface IGetGuidanceLinks
        {
            IEnumerable<LinkItem> GenerateLinkItems(string key);
        }

        public interface IGetResourcesLinks
        {
            IEnumerable<LinkItem> GenerateLinkItems(string key);
        }

        public class GetGuidanceLinks(IConfiguration configuration)
            : GetLinks(configuration, "GuidanceLookup"), IGetGuidanceLinks;

        public class GetResourcesLinks(IConfiguration configuration)
            : GetLinks(configuration, "ResourcesLookup"), IGetResourcesLinks;



        public class GetLinks(IConfiguration configuration, string LookupKey)
        {
            public IEnumerable<LinkItem> GenerateLinkItems(string key)
            {
                var guidanceItems = configuration.GetSection($"{LookupKey}:{key}").Get<IEnumerable<LinkItem>>();
                return guidanceItems ?? new List<LinkItem>();
            }
        }

    }
}
