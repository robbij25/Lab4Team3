using Lab2Maui.AirportModel;
using System.Collections.ObjectModel;
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

        public void TestDeleteAirport()
        {
            // Arrange
            IBusinessLogic businessLogic = new BusinessLogic();
            String id = "KATW";
            String city = "Appleton";
            DateTime date = new DateTime(2023, 3, 3, 14, 0, 0);
            int rating  = 3;

            // Act
            businessLogic.AddAirport(id, city, date, rating);
            bool removedAirport = businessLogic.DeleteAirport(id);

            // Assert
            Assert.True(removedAirport); // Checks if deletion was successful
        }

        public void TestRetrieveAllEntries()
        {
            // Arrange
            IBusinessLogic businessLogic = new BusinessLogic();
            ObservableCollection<Airport> expectedAirports = new ObservableCollection<Airport>();

            // Airport 1
            String airport1id = "KATW";
            String airport1city = "Appleton";
            DateTime airport1date = new DateTime(2023, 3, 3, 14, 0, 0);
            int airport1rating  = 3;
            Airport airport1 = new Airport(airport1id, airport1city, airport1date, airport1rating);

            // Airport 2
            String airport2id = "KFDL";
            String airport2city = "Fond du Lac";
            DateTime airport2date = DateTime.Now;
            int airport2rating = 4;
            Airport airport2 = new Airport(airport2id, airport2city, airport2date, airport2rating);

            // Act
            expectedAirports.Add(airport1);
            expectedAirports.Add(airport2);
            businessLogic.AddAirport(airport1id, airport1city, airport1date, airport1rating);
            businessLogic.AddAirport(airport2id, airport2city, airport2date, airport2rating);
            ObservableCollection<Airport> actualAirports = businessLogic.GetAirports();


            // Assert
            Assert.Equal(expectedAirports.Count, actualAirports.Count); // Check if counts are equal
            // Check that each element are equal
            for (int i = 0; i < expectedAirports.Count; i++)
            {
                Assert.Equal(expectedAirports[i].Id, actualAirports[i].Id);
                Assert.Equal(expectedAirports[i].City, actualAirports[i].City);
                Assert.Equal(expectedAirports[i].DateVisited, actualAirports[i].DateVisited);
                Assert.Equal(expectedAirports[i].Rating, actualAirports[i].Rating);
            }
        }
    }
}