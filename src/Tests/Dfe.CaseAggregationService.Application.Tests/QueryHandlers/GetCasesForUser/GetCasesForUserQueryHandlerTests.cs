using System.Security.Cryptography.Pkcs;
using AutoFixture;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Application.Services.Builders.Dfe.CaseAggregationService.Application.Services.Builders;
using Dfe.CaseAggregationService.Domain.Entities.Academisation;
using Dfe.CaseAggregationService.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.QueryHandlers.GetCasesForUser
{
    public class GetCasesForUserQueryHandlerTests
    {
        [Fact]
        public async Task Handle_GetCaseForUser()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName, userEmail, true, true, true, true, true, true, []);

            var academisation = Substitute.For<IGetAcademisationSummary>();
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            academisation.GetAcademisationSummaries(userEmail, false, false, false, null).Returns([
                fixture.Create<AcademisationSummary>()
            ]);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }



        [Fact]
        public async Task Handle_GetCaseForUser_SortingDesc()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.CreatedDateDescending);


            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var academisation = FixtureAcademisationSummary(fixture, userEmail);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            var getCasesByUserResponseModel = result.Value!;
            Assert.Equal(5, getCasesByUserResponseModel.TotalRecordCount);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos.Count);

            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos[0].CreatedDate.Day);
            Assert.Equal(4, getCasesByUserResponseModel.CaseInfos[1].CreatedDate.Day);
            Assert.Equal(3, getCasesByUserResponseModel.CaseInfos[2].CreatedDate.Day);
            Assert.Equal(2, getCasesByUserResponseModel.CaseInfos[3].CreatedDate.Day);
            Assert.Equal(1, getCasesByUserResponseModel.CaseInfos[4].CreatedDate.Day);
        }

        private static GetCaseInfoFromAcademisationSummary GetCaseInfoFromAcademisationSummary()
        {
            var getGuidanceLinks = Substitute.For<IGetGuidanceLinks>();
            getGuidanceLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getResourcesLinks = Substitute.For<IGetResourcesLinks>();
            getResourcesLinks.GenerateLinkItems(Arg.Any<string>()).Returns([]);

            var getSystemLinks = Substitute.For<IGetSystemLinks>();
            getSystemLinks.GetPrepareTitleLink(Arg.Any<string>()).Returns("http://TitleLink");

            var mapper = new GetCaseInfoFromAcademisationSummary(getGuidanceLinks, getResourcesLinks, getSystemLinks);
            return mapper;
        }

        [Fact]
        public async Task Handle_GetCaseForUser_SortingAsc()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.CreatedDateAscending);

            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var academisation = FixtureAcademisationSummary(fixture, userEmail);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);


            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert

            // Assert
            Assert.NotNull(result);

            var getCasesByUserResponseModel = result.Value!;
            Assert.Equal(5, getCasesByUserResponseModel.TotalRecordCount);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos.Count);

            Assert.Equal(1, getCasesByUserResponseModel.CaseInfos[0].CreatedDate.Day);
            Assert.Equal(2, getCasesByUserResponseModel.CaseInfos[1].CreatedDate.Day);
            Assert.Equal(3, getCasesByUserResponseModel.CaseInfos[2].CreatedDate.Day);
            Assert.Equal(4, getCasesByUserResponseModel.CaseInfos[3].CreatedDate.Day);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos[4].CreatedDate.Day);
        }

        [Fact]
        public async Task Handle_GetCaseForUser_SortingUpdatedDesc()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.UpdatedDateDescending);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var academisation = FixtureAcademisationSummary(fixture, userEmail);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert


            // Assert
            Assert.NotNull(result);

            var getCasesByUserResponseModel = result.Value!;
            Assert.Equal(5, getCasesByUserResponseModel.TotalRecordCount);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos.Count);

            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos[0].UpdatedDate.Day);
            Assert.Equal(4, getCasesByUserResponseModel.CaseInfos[1].UpdatedDate.Day);
            Assert.Equal(3, getCasesByUserResponseModel.CaseInfos[2].UpdatedDate.Day);
            Assert.Equal(2, getCasesByUserResponseModel.CaseInfos[3].UpdatedDate.Day);
            Assert.Equal(1, getCasesByUserResponseModel.CaseInfos[4].UpdatedDate.Day);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Handle_GetCaseForUser_SortingUpdatedAsc()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.UpdatedDateAscending);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var academisation = FixtureAcademisationSummary(fixture, userEmail);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            var getCasesByUserResponseModel = result.Value!;
            Assert.Equal(5, getCasesByUserResponseModel.TotalRecordCount);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos.Count);

            Assert.Equal(1, getCasesByUserResponseModel.CaseInfos[0].UpdatedDate.Day);
            Assert.Equal(2, getCasesByUserResponseModel.CaseInfos[1].UpdatedDate.Day);
            Assert.Equal(3, getCasesByUserResponseModel.CaseInfos[2].UpdatedDate.Day);
            Assert.Equal(4, getCasesByUserResponseModel.CaseInfos[3].UpdatedDate.Day);
            Assert.Equal(5, getCasesByUserResponseModel.CaseInfos[4].UpdatedDate.Day);
        }



        [Fact]
        public async Task Handle_GetCaseForUser_Pages()
        {
            // Arrange
            var fixture = new Fixture();
            var userName = fixture.Create<string>();
            var userEmail = fixture.Create<string>();
            var page1query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.UpdatedDateAscending,
                1,
                3);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var academisation = FixtureAcademisationSummary(fixture, userEmail);

            var caseInfoAcademisation = GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, caseInfoAcademisation, logger);

            // Act
            var resultPage1 = await handler.Handle(page1query, CancellationToken.None);

            Assert.NotNull(resultPage1);
            var resultPage1List = resultPage1.Value!;

            Assert.Equal(5, resultPage1List.TotalRecordCount);
            Assert.Equal(3, resultPage1List.CaseInfos.Count);

            Assert.Equal(1, resultPage1List.CaseInfos[0].UpdatedDate.Day);
            Assert.Equal(2, resultPage1List.CaseInfos[1].UpdatedDate.Day);
            Assert.Equal(3, resultPage1List.CaseInfos[2].UpdatedDate.Day);

            var page2query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                true,
                true,
                true,
                true,
                [
                ],
                null,
                SortCriteria.UpdatedDateAscending,
                2,
                3);


            var resultPage2 = await handler.Handle(page2query, CancellationToken.None);

            Assert.NotNull(resultPage2);
            var resultPage2List = resultPage2.Value!;

            Assert.Equal(5, resultPage2List.TotalRecordCount);
            Assert.Equal(2, resultPage2List.CaseInfos.Count);

            Assert.Equal(4, resultPage2List.CaseInfos[0].UpdatedDate.Day);
            Assert.Equal(5, resultPage2List.CaseInfos[1].UpdatedDate.Day);

        }


        private static IGetAcademisationSummary FixtureAcademisationSummary(Fixture fixture, string userEmail)
        {
            var academisation = Substitute.For<IGetAcademisationSummary>();

            var fix1 = fixture.Create<AcademisationSummary>();
            fix1.CreatedOn = new DateTime(2001, 01, 01);
            fix1.LastModifiedOn = new DateTime(2001, 01, 01);

            var fix2 = fixture.Create<AcademisationSummary>();
            fix2.CreatedOn = new DateTime(2001, 01, 02);
            fix2.LastModifiedOn = new DateTime(2001, 01, 02);

            var fix3 = fixture.Create<AcademisationSummary>();
            fix3.CreatedOn = new DateTime(2001, 01, 03);
            fix3.LastModifiedOn = new DateTime(2001, 01, 03);

            var fix4 = fixture.Create<AcademisationSummary>();
            fix4.CreatedOn = new DateTime(2001, 01, 04);
            fix4.LastModifiedOn = new DateTime(2001, 01, 04);

            var fix5 = fixture.Create<AcademisationSummary>();
            fix5.CreatedOn = new DateTime(2001, 01, 05);
            fix5.LastModifiedOn = new DateTime(2001, 01, 05);

            academisation.GetAcademisationSummaries(userEmail, false, false, false, null).Returns([
                fix5,
                fix4,
                fix3,
                fix2,
                fix1
            ]);
            return academisation;
        }

    }
}
