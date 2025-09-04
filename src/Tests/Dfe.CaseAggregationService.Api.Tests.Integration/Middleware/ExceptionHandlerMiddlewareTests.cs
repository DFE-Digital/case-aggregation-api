using Dfe.CaseAggregationService.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Api.Tests.Integration.Middleware
{
    public class ExceptionHandlerMiddlewareTests
    { 
        [Fact]
        public async Task InvokeAsync_WhenException_Returns500()
        {
            // Arrange
            var logger = Substitute.For<ILogger<ExceptionHandlerMiddleware>>();
            var middleware = new ExceptionHandlerMiddleware((_) => throw new Exception("Test exception"), logger);

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_WhenNoException_CallsNextDelegate()
        {
            // Arrange
            var logger = Substitute.For<ILogger<ExceptionHandlerMiddleware>>();
            var called = false;
            var middleware = new ExceptionHandlerMiddleware(async (_) =>
            {
                called = true;
                await Task.CompletedTask;
            }, logger);

            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(called);
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        }
    
    }
}
