using System.ComponentModel;
using System.Runtime.Serialization;

// This class represents an airport with an id, city, dateVisited and a rating.
namespace Lab2Maui.AirportModel;


public class Airport {

    private String? id;
    private String? city;
    private DateTime? dateVisited;
    private int rating;

    // Property for getting and setting the id field
    public String? Id {
        get { return id; }
        set { id = value; }
    }

    // Property for getting and setting the city field
    public String? City {
        get { return city; }
        set { city = value; }
    }



    // Property for getting and setting the date visited
    public DateTime? DateVisited {
        get { return dateVisited; }
        set { dateVisited = value; }
    }



    // Property for getting and setting the rating
    public int Rating {
        get { return rating; }
        set { rating = value; }
    }



    // A constructor that takes in the id, city, date visited and rating and sets
    // them for this airport
    public Airport(String id, String city, DateTime dateVisited, int rating) {
        Id = id;
        City = city;
        DateVisited = dateVisited;
        Rating = rating;
    }

    // Default constructor with default values for this airport
    public Airport() : this("KATW", "Appleton", DateTime.Now, 5) { }

    // This is the equals method that checks to see if the object being passed in
    // is a type of Airport and if it has the same id as this airport
    public override bool Equals(Object? obj) {
        return (obj is Airport someAirport && someAirport.Id == this.Id);
    }

    // Method that gets overridden so that Equals method will work
    public override int GetHashCode() {

        throw new NotImplementedException();

    }

    //overriding ToString method so that airports display nicely when they are printed to console
    public override string ToString() {
        return $"{id} - {city}, {dateVisited:MM/dd/yyyy}, {rating}";
    }

}