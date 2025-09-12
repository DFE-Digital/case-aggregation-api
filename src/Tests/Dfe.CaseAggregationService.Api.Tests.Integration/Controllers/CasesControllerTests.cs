using Dfe.CaseAggregationService.Client.Contracts;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Tests.Common.Customizations;
using GovUK.Dfe.CoreLibs.Testing.AutoFixture.Attributes;
using GovUK.Dfe.CoreLibs.Testing.Mocks.WebApplicationFactory;
using GovUK.Dfe.CoreLibs.Testing.Mocks.WireMock;
using System.Security.Claims;
using AutoFixture;
using Dfe.CaseAggregationService.Infrastructure.Dto.Mfsp;
using Dfe.CaseAggregationService.Infrastructure.Dto.Recast;
using Dfe.AcademiesApi.Client.Contracts;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Dfe.CaseAggregationService.Api.Tests.Integration.Controllers
{
    public class CasesControllerTests
    {
        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization))]
        public async Task GetCasesAsync_ShouldCases_WhenCaseExists(
            CustomWebApplicationDbContextFactory<Program> factory,
            ICasesClient caseClient,
            IFixture fixture)
        {
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            const string? conversionsSummarySchoolName = "Academy School Name";
            const string? transfersSummarySchoolName = "Transfer School Name";
            const string? mfspSummarySchoolName = "MfSP School Name";
            const string? trustName = "Test Trust Name";

            SetupPrepare(factory, fixture, conversionsSummarySchoolName, transfersSummarySchoolName);
            
            SetupMfsp(factory, fixture, mfspSummarySchoolName);

            SetupRecast(factory, fixture, (trustName, "Non-compliance"));

            // Act
            var result = await caseClient.GetCasesByUserAsync("userName",
                "userEmail",
                false,
                true,
                false,
                true,
                true,
                false,
                null,
                "",
                null,
                1,
                25,
                "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.TotalRecordCount);
            Assert.Equal(transfersSummarySchoolName, result.CaseInfos[0].Title);
            Assert.Equal(conversionsSummarySchoolName, result.CaseInfos[1].Title);
            Assert.Equal(mfspSummarySchoolName, result.CaseInfos[2].Title);
            Assert.Equal(trustName, result.CaseInfos[3].Title);
        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization))]
        public async Task GetCasesAsync_ShouldRecastCases_WhenCaseExists(
            CustomWebApplicationDbContextFactory<Program> factory,
            ICasesClient caseClient,
            IFixture fixture)
        {
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            const string? trustName = "Test Trust Name";
            const string? filteredTrust = "Filtered Trust Name";

            SetupRecast(factory, fixture, (trustName, "Non-compliance"), (filteredTrust, "Governance capability"));

            // Act
            var result = await caseClient.GetCasesByUserAsync("userName",
                "userEmail",
                false,
                false,
                false,
                false,
                true,
                false,
                ["Non-compliance"],
                "",
                null,
                1,
                25,
                "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalRecordCount);
            Assert.Equal(filteredTrust, result.CaseInfos[0].Title);
            Assert.DoesNotContain(result.CaseInfos, c => c.ProjectType == "Governance capability");

        }

        [Theory]
        [CustomAutoData(typeof(CustomWebApplicationDbContextFactoryCustomization))]
        public async Task GetCasesAsync_ShouldAcademisationCases_WhenCaseExists(
            CustomWebApplicationDbContextFactory<Program> factory,
            ICasesClient caseClient,
            IFixture fixture)
        {
            factory.TestClaims = [new Claim(ClaimTypes.Role, "API.Read")];

            const string? conversionsSummarySchoolName = "Academy School Name";
            const string? transfersSummarySchoolName = "Transfer School Name";

            SetupPrepare(factory, fixture, conversionsSummarySchoolName, transfersSummarySchoolName);

            // Act
            var result = await caseClient.GetCasesByUserAsync("userName",
                "userEmail",
                false,
                true,
                false,
                true,
                true,
                false,
                ["Conversion"],
                "",
                null,
                1,
                25,
                "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalRecordCount);
            Assert.Equal(transfersSummarySchoolName, result.CaseInfos.First().Title);
        }

        private static void SetupRecast(CustomWebApplicationDbContextFactory<Program> factory, IFixture fixture, params
            (string trustName, string caseType)[] recastCase)
        {

            var recastResponseData = new List<ActiveCaseSummaryResponse>();

            var recastResponse = new ApiResponseV2<ActiveCaseSummaryResponse>()
            {
                Data = recastResponseData,
                Paging = null
            };

            var counter = 1;

            var trustResponse = new ObservableCollection<TrustDto>();

            var ukprn = $"{Random.Shared.NextInt64(100, 999)}0{counter.ToString()}";

            var trust = fixture.Create<TrustDto>();

            trust.Ukprn = ukprn;

            trustResponse.Add(trust);

            foreach (var recast in recastCase)
            {
                var outputCase = fixture.Create<ActiveCaseSummaryResponse>();

                //var ukprn = $"{Random.Shared.NextInt64(100,999)}0{counter.ToString()}";

                outputCase.CreatedAt = DateTime.UtcNow.AddMonths(1);
                outputCase.TrustUkPrn = ukprn;

                outputCase.ActiveConcerns = new List<CaseSummaryResponse.Concern>()
                {
                    new (recast.caseType, new ConcernsRatingResponse(), DateTime.UtcNow.AddMonths(-1))
                };

                trust.Name = recast.trustName;

                recastResponseData.Add(outputCase);
            }
            
            Assert.NotNull(factory.WireMockServer);
            factory.WireMockServer.AddGetWithJsonResponse($"/recast/v2/concerns-cases/summary/userEmail/active", recastResponse, new List<KeyValuePair<string, string>> 
            {
                new("page", "1"),
                new("count", "100")
            });

            factory.WireMockServer.AddGetWithJsonResponse($"/academies/v4/trusts/bulk",
                trustResponse,
                trustResponse.Select(x => new KeyValuePair<string, string>("ukprns", x.Ukprn ?? "")).ToList());

            var request = Request.Create()
                .WithPath("/academies/v4/trusts/bulk")
                .UsingGet();

            foreach (var kvp in trustResponse.Select(x => new KeyValuePair<string, string>("ukprns", x.Ukprn ?? "")).ToList() ?? [])
            {
                request = request.WithParam(kvp.Key, kvp.Value);
            }

            factory.WireMockServer
                .Given(request)
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBody(JsonConvert.SerializeObject(trustResponse)));
        }

        private static void SetupMfsp(CustomWebApplicationDbContextFactory<Program> factory, IFixture fixture,
            string? mfspSummarySchoolName)
        {
           

            var mfspSummary = fixture.Create<GetProjectSummaryResponse>();

            mfspSummary.ProjectStatus = "Pre-opening";
            mfspSummary.UpdatedAt = DateTime.UtcNow.AddMonths(2);
            mfspSummary.ProjectTitle = mfspSummarySchoolName;

            var mfspResponse = new ApiListWrapper<GetProjectSummaryResponse>()
            {
                Data = new List<GetProjectSummaryResponse>() { mfspSummary },
                Paging = null
            };

            Assert.NotNull(factory.WireMockServer);
            factory.WireMockServer.AddGetWithJsonResponse($"/mfsp/api/v1/summary/project", mfspResponse, new List<KeyValuePair<string, string>> { new("projectManagedByEmail", "userEmail") });
        }

        private static void SetupPrepare(CustomWebApplicationDbContextFactory<Program> factory, IFixture fixture,
            string? conversionsSummarySchoolName,
            string? transferSummarySchoolName)
        {
            var conversionAcademySummary = new AcademisationSummary()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow.AddMonths(3),
                LastModifiedOn = DateTime.UtcNow.AddMonths(2),
                ConversionsSummary = fixture.Build<ConversionsSummary>()
                    .With(x => x.SchoolName, conversionsSummarySchoolName)
                    .With(x => x.ProjectStatus, "Deferred")
                    .Create()
            };

            var transferAcademySummary = new AcademisationSummary()
            {
                Id = 1,
                CreatedOn = DateTime.UtcNow.AddMonths(4),
                LastModifiedOn = DateTime.UtcNow.AddMonths(3),
                TransfersSummary = fixture.Build<TransfersSummary>()
                    .With(x => x.Status, (string?)null)
                    .With(x => x.IncomingTrustName, transferSummarySchoolName)
                    .Create()
            };

            Assert.NotNull(factory.WireMockServer);
            factory.WireMockServer.AddGetWithJsonResponse($"/academisation/summary/projects", new[] { conversionAcademySummary, transferAcademySummary }, new List<KeyValuePair<string, string>> { new("email", "userEmail") });

            factory.WireMockServer.AddGetWithJsonResponse($"/conversion-project/formamatproject/123", new[] { conversionAcademySummary, transferAcademySummary });
        }
    }
}
