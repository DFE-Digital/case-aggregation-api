using AutoFixture;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.SystemIntegration;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Dfe.CaseAggregationService.Application.Tests.QueryHandlers.GetCasesForUser
{
    public class GetCasesForUserQueryHandlerTests
    {

        private readonly Fixture _fixture;

        public GetCasesForUserQueryHandlerTests()
        {
            _fixture = new Fixture();

        }

        [Fact]
        public async Task Handle_GetCaseForUser()
        {
            // Arrange
            var userName = _fixture.Create<string>();
            var userEmail = _fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName, userEmail, true, false, false, false, false, false, []);


            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();
            
            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);
            
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
                false,
                false,
                false,
                false,
                [
                ],
                null,
                SortCriteria.CreatedDateDescending);


            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);

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
                false,
                false,
                false,
                false,
                [
                ],
                null,
                SortCriteria.CreatedDateAscending);

            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();
            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);

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
                false,
                false,
                false,
                false,
                [
                ],
                null,
                SortCriteria.UpdatedDateDescending);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            
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
                false,
                false,
                false,
                false,
                [
                ],
                null,
                SortCriteria.UpdatedDateAscending);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);
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
                false,
                false,
                false,
                false,
                [
                ],
                null,
                SortCriteria.UpdatedDateAscending,
                1,
                3);
            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();

            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);
            // Act
            var resultPage1 = await handler.Handle(page1query, CancellationToken.None);

            Assert.NotNull(resultPage1);
            var resultPage1List = resultPage1.Value!;

            Assert.NotNull(resultPage1List);

            Assert.Equal(5, resultPage1List.TotalRecordCount);
            Assert.Equal(3, resultPage1List.CaseInfos?.Count);

            Assert.Equal(1, resultPage1List.CaseInfos?[0].UpdatedDate.Day);
            Assert.Equal(2, resultPage1List.CaseInfos?[1].UpdatedDate.Day);
            Assert.Equal(3, resultPage1List.CaseInfos?[2].UpdatedDate.Day);

            var page2query = new GetCasesForUserQuery(userName,
                userEmail,
                false,
                true,
                false,
                false,
                false,
                false,
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


        private ISystemIntegration FixtureIntegration(string userEmail)
        {
            var integration = Substitute.For<ISystemIntegration>();

            // Create UserCaseInfo using constructor to set CreatedDate and UpdatedDate
            var fix1 = FixtureUserCaseInfo(1);

            var fix2 = FixtureUserCaseInfo(2);

            var fix3 = FixtureUserCaseInfo(3);

            var fix4 = FixtureUserCaseInfo(4);

            var fix5 = FixtureUserCaseInfo(5);

            integration.GetCasesForQuery(Arg.Is<GetCasesForUserQuery>(x => x.UserEmail == userEmail), Arg.Any<CancellationToken>()).Returns([
                fix5,
                fix4,
                fix3,
                fix2,
                fix1
            ]);
            return integration;
        }

        private UserCaseInfo FixtureUserCaseInfo(int day)
        {
            return _fixture.Build<UserCaseInfo>()
                .With(p => p.CreatedDate, new DateTime(2001, 1, day))
                .With(p => p.UpdatedDate, new DateTime(2001, 1, day)).Create();
        }
        [Fact]
        public async Task Handle_GetCaseForUser_NewTestToChange()
        {
            // Arrange
            var userName = _fixture.Create<string>();
            var userEmail = _fixture.Create<string>();
            var query = new GetCasesForUserQuery(userName, userEmail, false, false, false, false, false, false, []);

            var logger = Substitute.For<ILogger<GetCasesForUserQueryHandler>>();
            var handler = new GetCasesForUserQueryHandler([FixtureIntegration(userEmail)], logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);
            // Assert
            Assert.NotNull(result);
            // TODO: Add more assertions or modify this test as needed.
        }
    }
}
