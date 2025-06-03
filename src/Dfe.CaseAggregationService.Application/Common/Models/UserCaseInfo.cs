using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;

namespace Dfe.CaseAggregationService.Application.Common.Models
{
    public class UserCaseInfo(IEnumerable<CaseInfoItem> info, string title, string titleLink, string system, string projectType, DateTime createdDate, DateTime updatedDate)
    {
        public string Title { get; init; } = title;
        public string TitleLink { get; init; } = titleLink;
        public string System { get; init; } = system;
        public string ProjectType { get; init; } = projectType;
        public DateTime CreatedDate { get; init; } = createdDate;
        public DateTime UpdatedDate { get; init; } = updatedDate;

        public IEnumerable<CaseInfoItem> Info { get; init; } = info;
    }

    public record CaseInfoItem(string Label, string? Value, string? Link);
}
