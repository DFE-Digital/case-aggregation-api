using System.Net;
using System.Text;
using Dfe.CaseAggregationService.Infrastructure.Dto.Mfsp;
using Dfe.CaseAggregationService.Infrastructure.Gateways;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace Dfe.CaseAggregationService.Infrastructure.Tests.Gateways
{
    public class MfspApiClientTests
    {
        [Fact]
        public async Task GetMfspSummaries_RequestsPageOneWithSourceCapOf100()
        {
            var handler = new RecordingHandler(Json(Wrap([Summary("T1", "Presumption")])));
            var client = CreateClient(handler);

            await client.GetMfspSummaries("user@education.gov.uk", null);

            Assert.NotNull(handler.LastRequestUri);
            Assert.Contains("page=1", handler.LastRequestUri);
            Assert.Contains("count=100", handler.LastRequestUri);
            Assert.Contains("projectManagedByEmail=", handler.LastRequestUri);
            Assert.Contains("user@education.gov.uk", Uri.UnescapeDataString(handler.LastRequestUri));
        }

        [Fact]
        public async Task GetMfspSummaries_WhenDataIsNull_ReturnsEmpty()
        {
            var client = CreateClient(new RecordingHandler("""{"data":null}"""));

            var result = await client.GetMfspSummaries("user@education.gov.uk", null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMfspSummaries_WhenDataIsEmpty_ReturnsEmpty()
        {
            var client = CreateClient(new RecordingHandler(Json(Wrap([]))));

            var result = await client.GetMfspSummaries("user@education.gov.uk", null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetMfspSummaries_SkipsRowsWithoutProjectId()
        {
            var client = CreateClient(new RecordingHandler(Json(Wrap(
            [
                Summary(null, "Presumption"),
                Summary("", "Presumption"),
                Summary("T3", "Presumption")
            ]))));

            var result = (await client.GetMfspSummaries("user@education.gov.uk", null)).ToList();

            Assert.Single(result);
            Assert.Equal("T3", result[0].ProjectId);
        }

        [Fact]
        public async Task GetMfspSummaries_WhenFilteredByPresumption_KeepsMatchingType()
        {
            var client = CreateClient(new RecordingHandler(Json(Wrap(
            [
                Summary("T1", "Presumption"),
                Summary("T2", "Central Route")
            ]))));

            var result = (await client.GetMfspSummaries("user@education.gov.uk", ["Presumption"])).ToList();

            Assert.Single(result);
            Assert.Equal("T1", result[0].ProjectId);
            Assert.Equal("Presumption", result[0].ProjectType);
        }

        [Fact]
        public async Task GetMfspSummaries_WhenFilteredByCentralRoute_ExcludesPresumption()
        {
            var client = CreateClient(new RecordingHandler(Json(Wrap([Summary("T1", "Presumption")]))));

            var result = await client.GetMfspSummaries("user@education.gov.uk", ["Central Route"]);

            Assert.Empty(result);
        }

        private static MfspApiClient CreateClient(RecordingHandler handler)
        {
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://mfsp.test/")
            };
            var factory = Substitute.For<IHttpClientFactory>();
            factory.CreateClient(Arg.Any<string>()).Returns(httpClient);
            var logger = Substitute.For<ILogger<ApiClient>>();

            return new MfspApiClient(factory, logger);
        }

        private static ApiListWrapper<GetProjectSummaryResponse> Wrap(List<GetProjectSummaryResponse> data) =>
            new() { Data = data, Paging = null };

        private static GetProjectSummaryResponse Summary(string? projectId, string projectType) =>
            new()
            {
                ProjectId = projectId,
                ProjectType = projectType,
                ProjectTitle = "School",
                ProjectStatus = "Pre-opening"
            };

        private static string Json(object value) => JsonConvert.SerializeObject(value);

        private sealed class RecordingHandler(string json) : HttpMessageHandler
        {
            public string? LastRequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                LastRequestUri = request.RequestUri is { IsAbsoluteUri: true } uri
                    ? uri.AbsoluteUri
                    : request.RequestUri?.OriginalString;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });
            }
        }
    }
}
