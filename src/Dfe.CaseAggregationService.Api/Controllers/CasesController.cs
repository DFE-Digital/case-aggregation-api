using Asp.Versioning;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Schools.Commands.CreateReport;
using Dfe.CaseAggregationService.Application.Schools.Commands.CreateSchool;
using Dfe.CaseAggregationService.Application.Schools.Queries.GetPrincipalBySchool;
using Dfe.CaseAggregationService.Application.Schools.Queries.GetPrincipalsBySchools;
using Dfe.CaseAggregationService.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Exceptions;
using Dfe.CaseAggregationService.Infrastructure.Security.Configurations;

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
        /// <param name="schoolName">The school name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        //[Authorize(Policy = PolicyNames.CanRead)]
        [HttpGet("/user/")]
        [SwaggerResponse(200, "A Person object representing the Principal.", typeof(Principal))]
        [SwaggerResponse(404, "School not found.")]
        [SwaggerResponse(400, "School cannot be null or empty.")]
        public async Task<IActionResult> GetPrincipalBySchoolAsync([FromQuery] string userEmail, [FromQuery] string userName, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetCasesForUserQuery(userName, userEmail), cancellationToken);

            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
        }

    }
}