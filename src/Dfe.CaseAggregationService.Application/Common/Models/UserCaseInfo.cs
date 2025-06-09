namespace Dfe.CaseAggregationService.Application.Common.Models
{
    public class UserCaseInfo(
        string title,
        string titleLink,
        string system,
        string projectType,
        DateTime createdDate,
        DateTime updatedDate,
        IEnumerable<CaseInfoItem> info,
        IEnumerable<LinkItem> guidance,
        IEnumerable<LinkItem> resources)
    {
        public string Title { get; init; } = title;
        public string TitleLink { get; init; } = titleLink;
        public string System { get; init; } = system;
        public string ProjectType { get; init; } = projectType;
        public DateTime CreatedDate { get; init; } = createdDate;
        public DateTime UpdatedDate { get; init; } = updatedDate;

        public IEnumerable<CaseInfoItem> Info { get; init; } = info;
        public IEnumerable<LinkItem> Guidance { get; init; } = guidance;
        public IEnumerable<LinkItem> Resources { get; init; } = resources;

    }

    public record CaseInfoItem(string Label, string? Value, string? Link);
    public record LinkItem(string Title, string? Link);
}
