using Asp.Versioning;
using Dfe.CaseAggregationService.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Exceptions;
using Dfe.CaseAggregationService.Infrastructure.Security.Configurations;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Policy = PolicyNames.CanRead)]
        [HttpGet("/user/")]
        [SwaggerResponse(200, "Projects and Cases for the provided user.", typeof(GetCasesByUserResponseModel))]
        [SwaggerResponse(400, "User name cannot be null or empty.")]
        [SwaggerResponse(400, "User email cannot be null or empty.")]
        public async Task<IActionResult> GetCasesByUser([FromQuery] GetCasesForUserQuery query,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);

            return !result.IsSuccess ? NotFound(new CustomProblemDetails(HttpStatusCode.NotFound, result.Error)) : Ok(result.Value);
        }

    }
}