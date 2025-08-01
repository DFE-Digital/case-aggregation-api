using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Common.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class PerformanceBehaviour<TRequest, TResponse>(
        ILogger<TRequest> logger,
        IHttpContextAccessor httpContextAccessor)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Stopwatch _timer = new();

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;

            var user = httpContextAccessor.HttpContext?.User;

            var requestName = typeof(TRequest).Name;
            var identityName = user?.Identity?.Name;

            logger.LogWarning("CaseAggregationAPI Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@IdentityName} {@Request}",
                requestName, elapsedMilliseconds, identityName, request);

            return response;
        }
    }

}
