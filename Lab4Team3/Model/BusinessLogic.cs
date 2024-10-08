using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.InteropServices;
using Lab2Maui.AirportModel;

namespace Lab2Maui.AirportModel
//import statement for database layer needed I think
{
    public static class DbAccessStatus
    {
        public static readonly string DuplicateValue = "This value already exists";
        public static readonly string NameTooLong = "The city name is too long";
        public static readonly string IDWrongSize = "The ID must be 3 or 4 characters in length.";
        public static readonly string InvalidDate = "Your date either hasn't happened, or happened before powered flight.";
        public static readonly string InvalidRating = "Your rating must be between 1 and 5 (inclusive).";
        public static readonly string Success = "_succeeded_";
        public static readonly string EmptyField = "All fields must be filled out.";

    }
    public class BusinessLogic : IBusinessLogic
    {

        private ObservableCollection<Airport> airports;

        IDatabase dbLayer = new Database();
        public ObservableCollection<Airport> Airports { get { return dbLayer.SelectAllAirports(); } }

        //gets existing airports from database
        public BusinessLogic()
        {
            airports = dbLayer.SelectAllAirports();
        }

        //creates an Airport entry from input data and adds it to the database
        public string AddAirport(String id, String city, DateTime dateVisited, int rating)
        {
            string finishCode = DbAccessStatus.DuplicateValue;
            if (id == null || city == null || dateVisited == null || rating == null) return DbAccessStatus.EmptyField;
            //checks if the id length is appropriate
            if (id.Length == 3 || id.Length == 4)
            {
                //checks if the city name's length is appropriate
                if (city.Length <= 25)
                {
                    //checks if it's possible for the user to have visted an airport during this time
                    var thisDay = DateTime.Today;
                    if (dateVisited.CompareTo(new DateTime(1903, 12, 17)) > 0 && dateVisited.CompareTo(thisDay) < 0)
                    {
                        if (rating <= 5 && rating >= 1)
                        {
                            //create new airport with received information (if it is valid) and calls helper method to add
                            //it to the database
                            Airport visit = new Airport(id, city, dateVisited, rating);
                            if (dbLayer.InsertAirport(visit)) finishCode = DbAccessStatus.Success;
                            return finishCode;
                        }
                        else finishCode = DbAccessStatus.InvalidRating;
                    }
                    else finishCode = DbAccessStatus.InvalidDate;
                }
                else finishCode = DbAccessStatus.NameTooLong;
            }
            else finishCode = DbAccessStatus.IDWrongSize;
            return finishCode;
        }

        //removes selected airport from the database
        public bool DeleteAirport(String id)
        {
            bool removedAirport = false;
            for (int i = 0; i < airports.Count(); i++)
            {
                if (airports[i].Id == id)
                {
                    //remove the airport by calling the DB interface, and remove from class ObservableCollection too
                    removedAirport = (bool)dbLayer.DeleteAirport(id);
                }
            }
            if (removedAirport) airports = dbLayer.SelectAllAirports();
            return removedAirport;
        }

        //modify the information in the entry of a given airport (selected by id)
        public bool EditAirport(String id, String city, DateTime dateVisited, int rating)
        {
            bool editedAirport = false;
            //find the airport entry to be modified
            if (id == null || city == null || dateVisited == null || rating == null) return editedAirport;
            else {
                for (int i = 0; i < airports.Count(); i++) {
                    if (airports[i].Id == id)
                    {
                        Airport newEntry = new Airport(id, city, dateVisited, rating);
                        //update the boolean to reflect the change
                        editedAirport = dbLayer.UpdateAirport(newEntry);
                        airports = dbLayer.SelectAllAirports();
                        return editedAirport;
                    }
            }
            return editedAirport;}
        }

        //finds the airport requested by the id and return it
        public Airport FindAirport(String id)
        {
            for (int i = 0; i < airports.Count(); i++)
            {
                if (airports[i].Id == id) return airports[i];
            }
            return null;
        }

        //calculates a couple of stats and generates a message to write out
        public String CalculateStatistics()
        {
            const int BronzeScore = 41, SilverScore = 82, GoldScore = 123;
            const String Bronze = "Bronze", Silver = "Silver", Gold = "Gold";
            int airportsVisited = airports.Count();
            int remaining;
            //report how many airports have been visited
            String message = $"{airportsVisited} airports visited; ";
            String nextLevel;

            //figure out what the next level is and how many more airports must be visted to reach it
            if (airportsVisited < BronzeScore)
            {
                remaining = airportsVisited - BronzeScore;
                nextLevel = Bronze;
            }
            else if (airportsVisited < SilverScore)
            {
                remaining = airportsVisited - SilverScore;
                nextLevel = Silver;
            }
            else if (airportsVisited < GoldScore)
            {
                remaining = airportsVisited - GoldScore;
                nextLevel = Gold;
            }
            else { nextLevel = null; remaining = 0; }
            if (nextLevel == null) message += "you already have a gold status!";
            else message += $"{remaining * -1} airports remaining until achieving {nextLevel}.";
            return message;
        }

        //returns the ObservableCollection of airports
        public ObservableCollection<Airport> GetAirports()
        {
            return dbLayer.SelectAllAirports();
        }
    }
}
