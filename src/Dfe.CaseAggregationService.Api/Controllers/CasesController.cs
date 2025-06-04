using Asp.Versioning;
using Dfe.CaseAggregationService.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Exceptions;
using Dfe.CaseAggregationService.Api.ResponseModels;

namespace Dfe.CaseAggregationService.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CasesController(ISender sender) : ControllerBase
    {
        /// <summary>
        /// Retrieve Principal by school name
        /// </summary>
        //[Authorize(Policy = PolicyNames.CanRead)]
        [HttpGet("/user/")]
        [SwaggerResponse(200, "A Person object representing the Principal.", typeof(GetCasesByUserResponseModel))]
        [SwaggerResponse(404, "School not found.")]
        [SwaggerResponse(400, "School cannot be null or empty.")]
        public async Task<IActionResult> GetCasesByUser([FromQuery] string userEmail,
            [FromQuery] string userName,
            [FromQuery] bool includeSignificantChange,
            [FromQuery] bool includePrepare,
            [FromQuery] bool includeComplete,
            [FromQuery] bool includeManageFreeSchools,
            [FromQuery] bool includeConcerns,
            [FromQuery] bool includeWarningNotices,
            [FromQuery] string? searchTerm,
            [FromQuery] string[] filterProjectTypes,
            [FromQuery] SortCriteria sortCriteria,
            [FromQuery] int page,
            [FromQuery] int recordCount,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetCasesForUserQuery(userName, userEmail, includeSignificantChange, includePrepare, includeComplete, includeManageFreeSchools, includeConcerns, includeWarningNotices, filterProjectTypes, searchTerm, sortCriteria, page, recordCount), cancellationToken);

            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
        }

    }
}