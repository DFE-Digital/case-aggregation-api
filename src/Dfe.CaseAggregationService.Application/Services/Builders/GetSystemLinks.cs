using Microsoft.Extensions.Configuration;

namespace Dfe.CaseAggregationService.Application.Services.Builders
{
    public interface IGetSystemLinks
    {
        string GetPrepareConversionTitleLink(params object[] formatKeys);

        string GetPrepareTransferTitleLink(params object[] formatKeys);

        string GetRecastTitleLink(params object[] formatKeys);

        string GetMfspTitleLink(params object[] formatKeys);

        string GetCompleteTitleLink(params object[] formatKeys);

        string GetPrepareFormAMatTitleLink(params object[] formatKeys);
    }

    public class GetSystemLinks(IConfiguration configuration): IGetSystemLinks
    {
        public string GetPrepareConversionTitleLink(params object[] formatKeys)
        {
            return BuildLink("PrepareConversionLink", formatKeys);
        }

        public string GetPrepareTransferTitleLink(params object[] formatKeys)
        {
            return BuildLink("PrepareTransferLink", formatKeys);
        }

        public string GetPrepareFormAMatTitleLink(params object[] formatKeys)
        {
            return BuildLink("PrepareFormAMatLink", formatKeys);
        }

        public string GetRecastTitleLink(params object[] formatKeys)
        {
            return BuildLink("RecastLink", formatKeys);
        }

        public string GetMfspTitleLink(params object[] formatKeys)
        {
            return BuildLink("MfspLink", formatKeys);
        }

        public string GetCompleteTitleLink(params object[] formatKeys)
        {
            return BuildLink("CompleteLink", formatKeys);
        }

        private string BuildLink(string linkKey, object[] formatKeys)
        {
            var linkBase = configuration.GetSection($"SystemLinks:{linkKey}").Get<string>();
            if (string.IsNullOrEmpty(linkBase))
                return string.Empty;

            return string.Format(linkBase, formatKeys);
        }
    }
}
