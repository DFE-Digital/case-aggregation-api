using System.Collections.ObjectModel;
using AutoFixture;
using AutoMapper;
using Dfe.CaseAggregationService.Application.Cases.Queries.GetCasesForUser;
using Dfe.CaseAggregationService.Application.MappingProfiles;
using Dfe.SignificantChange.Client.Contracts;
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
            var query = new GetCasesForUserQuery(userName, userEmail);
            var caseClient = Substitute.For<ICaseClient>();

            caseClient.GetSignificantChangeByUserAsync(userName).Returns(new ObservableCollection<SignificantChangeCase>
            {
                fixture.Create<SignificantChangeCase>(),
            });

            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<SignificantChangeProfile>()));
            var handler = new GetCasesForUserQueryHandler(caseClient, null, mapper);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }
    }
}
