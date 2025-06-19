using AutoFixture;
using Dfe.CaseAggregationService.Client.Contracts;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Tests.Common.Customizations;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.Mocks.WebApplicationFactory;
using DfE.CoreLibs.Testing.Mocks.WireMock;
using System.Security.Claims;

namespace Dfe.CaseAggregationService.Api.Tests.Integration.Controllers
{
    public class CasesControllerTests
    {
        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization))]
        public async Task GetCasesAsync_ShouldCases_WhenCaseExists(
            CustomWebApplicationDbContextFactory<Program> factory,
            ICasesClient caseClient)
        {
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            var academySummary = new AcademisationSummary()
            {
                Id = 1,
                Urn = 123456,
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now,
                ConversionsSummary = new ConversionsSummary()
                {
                    ApplicationReferenceNumber = "APP123456",
                    SchoolName = "Test School",
                    LocalAuthority = "Test Local Authority",
                    Region = "Test Region",
                    AcademyTypeAndRoute = "Converter",
                    NameOfTrust = "Test Trust",
                    AssignedUserEmailAddress = "user@test.com",
                    AssignedUserFullName = "Test User",
                    ProjectStatus = "Open",
                    TrustReferenceNumber = "TR123456",
                    CreatedOn = DateTime.UtcNow,
                    Decision = "Approved",
                    ConversionTransferDate = DateTime.UtcNow.AddMonths(1)
                }
            };
            
            Assert.NotNull(factory.WireMockServer);

            factory.WireMockServer.AddGetWithJsonResponse($"/summary/projects", new[] { academySummary }, new List<KeyValuePair<string, string>> { new("email", "userEmail") });

            // Act
            var result = await caseClient.GetCasesByUserAsync("userEmail", "userName", false, true, false, false, false, false, null, [], null, 1, 25, "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.TotalRecordCount, 1);
            Assert.Equal(result.CaseInfos.First().Title , academySummary.ConversionsSummary.SchoolName);
        }
    }
}
