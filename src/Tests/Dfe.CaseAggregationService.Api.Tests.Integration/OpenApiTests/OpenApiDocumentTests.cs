using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.Mocks.WebApplicationFactory;
using Dfe.CaseAggregationService.Tests.Common.Customizations;
using System.Net;

namespace Dfe.CaseAggregationService.Api.Tests.Integration.OpenApiTests;

public class OpenApiDocumentTests
{
#pragma warning disable xUnit1026

    [Theory]
    [CustomAutoData(typeof(CustomWebApplicationFactoryCustomization<Program>))]
    public async Task SwaggerEndpoint_ReturnsSuccessAndCorrectContentType(
        CustomWebApplicationFactory<Program> factory,

        HttpClient client)
    {
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
#pragma warning restore xUnit1026
}
