using Lab2Maui.AirportModel;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void ExampleTest()
        {
            // Arrange
            int a = 5;
            int b = 10;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(15, result);
        }

        public void TestAddDuplicateAirport()
        {
            // Arrange
            IBusinessLogic businessLogic = new BusinessLogic();
            String id = "KATW";
            String city = "Appleton";
            DateTime date = new DateTime(2023, 3, 3, 14, 0, 0);
            int rating  = 3;

            // Act
            String firstAddResult = businessLogic.AddAirport(id, city, date, rating);
            String duplicateAddResult = businessLogic.AddAirport(id, city, date, rating);

            // Assert
            Assert.Equal(DbAccessStatus.DuplicateValue, duplicateAddResult);
        }

        public void TestUpdateAirport()
        {
            // Arrange
            IBusinessLogic businessLogic = new BusinessLogic();
            String id = "KATW";
            String city = "Appleton";
            DateTime date = new DateTime(2023, 3, 3, 14, 0, 0);
            int rating  = 3;
            int newRating = 5;

            // Act
            businessLogic.AddAirport(id, city, date, rating);
            businessLogic.EditAirport(id, city, date, newRating);

            // Assert
            Assert.Equal(newRating, businessLogic.FindAirport(id).Rating);
        }
    }
}