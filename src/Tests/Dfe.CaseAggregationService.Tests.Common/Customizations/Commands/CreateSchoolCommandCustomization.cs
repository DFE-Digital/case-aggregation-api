using AutoFixture;
using Dfe.CaseAggregationService.Application.Schools.Commands.CreateSchool;
using Dfe.CaseAggregationService.Application.Schools.Models;

namespace Dfe.CaseAggregationService.Tests.Common.Customizations.Commands
{
    public class CreateSchoolCommandCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<DateOnly?>(composer => composer.FromFactory(() =>
            {
                var generateNull = fixture.Create<bool>();
                if (generateNull)
                {
                    return null;
                }
                else
                {
                    var safeDateTime = fixture.Create<DateTime>().Date;
                    return DateOnly.FromDateTime(safeDateTime);
                }
            }));

            fixture.Customize<CreateSchoolCommand>(composer => composer.FromFactory(() =>
            {
                var nameDetails = new NameDetailsModel{
                   FirstName = "John",
                   LastName = "Doe",
                   MiddleName = ""
                };

                var lastRefresh = fixture.Create<DateTime>().Date;
                if (lastRefresh > DateTime.Now.Date)
                {
                    lastRefresh = DateTime.Now.Date.AddDays(-1);
                }

                return new CreateSchoolCommand(
                    fixture.Create<string>(),
                    lastRefresh,
                    fixture.Create<DateOnly?>(),
                    nameDetails,
                    fixture.Create<PrincipalDetailsModel>()
                );
            }));
        }
    }
}
