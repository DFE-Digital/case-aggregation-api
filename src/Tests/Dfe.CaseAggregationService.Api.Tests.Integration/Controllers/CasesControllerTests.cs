using Dfe.CaseAggregationService.Client.Contracts;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Tests.Common.Customizations;
using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using DfE.CoreLibs.Testing.Mocks.WebApplicationFactory;
using DfE.CoreLibs.Testing.Mocks.WireMock;
using System.Security.Claims;
using Dfe.CaseAggregationService.Infrastructure.Dto.Mfsp;

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
                CreatedOn = DateTime.UtcNow.AddMonths(2),
                LastModifiedOn = DateTime.UtcNow.AddMonths(2),
                ConversionsSummary = new ConversionsSummary()
                {
                    Urn = 123456,
                    ApplicationReferenceNumber = "APP123456",
                    SchoolName = "Test School",
                    LocalAuthority = "Test Local Authority",
                    Region = "Test Region",
                    AcademyTypeAndRoute = "Converter",
                    NameOfTrust = "Test Trust",
                    AssignedUserEmailAddress = "user@test.com",
                    AssignedUserFullName = "Test User",
                    ProjectStatus = "Deferred",
                    TrustReferenceNumber = "TR123456",
                    CreatedOn = DateTime.UtcNow,
                    Decision = "Approved",
                    ConversionTransferDate = DateTime.UtcNow.AddMonths(2)
                }
            };

            var mfspSummary = new GetProjectSummaryResponse()
            {
                ProjectTitle = "Free school",
                ProjectId = "F12345",
                TrustName = "Magic Trust",
                Region = "Region",
                LocalAuthority = "LA",
                RealisticOpeningYear = "25/2026",
                ProjectStatus = "Pre-opening",
                ProjectManagedBy = "User Email",
                ProjectType = "Presumption",
                ProjectManagedByEmail = "userEmail",
                SchoolType = "A type",
                UpdatedAt = DateTime.UtcNow.AddMonths(1)
            };

            var mfspResponse = new ApiListWrapper<GetProjectSummaryResponse>()
            {
                Data = new List<GetProjectSummaryResponse>() { mfspSummary },
                Paging = null
            };

            Assert.NotNull(factory.WireMockServer);
            
            factory.WireMockServer.AddGetWithJsonResponse($"/academisation/summary/projects", new[] { academySummary }, new List<KeyValuePair<string, string>> { new("email", "userEmail") });

            factory.WireMockServer.AddGetWithJsonResponse($"/mfsp/api/v1/summary/project", mfspResponse, new List<KeyValuePair<string, string>> { new("projectManagedByEmail", "userEmail") });

            // Act
            var result = await caseClient.GetCasesByUserAsync("userName", "userEmail", false, true, false, true, false, false, null, "", null, 1, 25, "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecordCount);
            Assert.Equal(academySummary.ConversionsSummary.SchoolName, result.CaseInfos.First().Title);
            Assert.Equal(mfspSummary.ProjectTitle, result.CaseInfos[1].Title);
        }
    }
}
