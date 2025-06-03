using AutoFixture;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.Services.Builders;
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

            var handler = new GetCasesForUserQueryHandler(academisation, new GetCaseInfoFromAcademisationSummary(), logger);
            
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

            var mapper = new GetCaseInfoFromAcademisationSummary();
            var handler = new GetCasesForUserQueryHandler(academisation, new GetCaseInfoFromAcademisationSummary(), logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result.Value!.Count);

            Assert.Equal(5, result.Value!.ToArray()[0].CreatedDate.Day);
            Assert.Equal(4, result.Value!.ToArray()[1].CreatedDate.Day);
            Assert.Equal(3, result.Value!.ToArray()[2].CreatedDate.Day);
            Assert.Equal(2, result.Value!.ToArray()[3].CreatedDate.Day);
            Assert.Equal(1, result.Value!.ToArray()[4].CreatedDate.Day);
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

            var handler = new GetCasesForUserQueryHandler(academisation, new GetCaseInfoFromAcademisationSummary(), logger);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result.Value!.Count);

            Assert.Equal(1, result.Value!.ToArray()[0].CreatedDate.Day);
            Assert.Equal(2, result.Value!.ToArray()[1].CreatedDate.Day);
            Assert.Equal(3, result.Value!.ToArray()[2].CreatedDate.Day);
            Assert.Equal(4, result.Value!.ToArray()[3].CreatedDate.Day);
            Assert.Equal(5, result.Value!.ToArray()[4].CreatedDate.Day);
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

            var handler = new GetCasesForUserQueryHandler(academisation, new GetCaseInfoFromAcademisationSummary(), logger);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result.Value!.Count);

            Assert.Equal(5, result.Value!.ToArray()[0].UpdatedDate.Day);
            Assert.Equal(4, result.Value!.ToArray()[1].UpdatedDate.Day);
            Assert.Equal(3, result.Value!.ToArray()[2].UpdatedDate.Day);
            Assert.Equal(2, result.Value!.ToArray()[3].UpdatedDate.Day);
            Assert.Equal(1, result.Value!.ToArray()[4].UpdatedDate.Day);
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

            var handler = new GetCasesForUserQueryHandler(academisation, new GetCaseInfoFromAcademisationSummary(), logger);
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(5, result.Value!.Count);

            Assert.Equal(1, result.Value!.ToArray()[0].UpdatedDate.Day);
            Assert.Equal(2, result.Value!.ToArray()[1].UpdatedDate.Day);
            Assert.Equal(3, result.Value!.ToArray()[2].UpdatedDate.Day);
            Assert.Equal(4, result.Value!.ToArray()[3].UpdatedDate.Day);
            Assert.Equal(5, result.Value!.ToArray()[4].UpdatedDate.Day);
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
