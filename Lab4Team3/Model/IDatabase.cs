using System;
using System.Collections.ObjectModel;
namespace Lab2Maui.AirportModel;

public interface IDatabase
{
 // Selects all airports from a file and puts it into a variable
    ObservableCollection<Airport>? SelectAllAirports();
    // Selects an airport depending on the id
    Airport? SelectAirport(String id);
    // Inserts an airport depending on the airport passed in
    bool InsertAirport(Airport airport);
    // Deletes an airport depending on the airport id
    bool DeleteAirport(String id);
    // Updates an airport depending on the airport passed in
    bool UpdateAirport(Airport airport);
}
