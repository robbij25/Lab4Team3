using System;
using System.Collections.ObjectModel;

namespace Lab2Maui.AirportModel;

public interface IBusinessLogic
{
        public ObservableCollection<Airport> Airports { get; }
        public string AddAirport(String id, String city, DateTime dateVisited, int rating);
        public bool DeleteAirport(String id);
        public bool EditAirport(String id, String city, DateTime dateVisited, int rating);
        public Airport FindAirport(String id);
        public String CalculateStatistics();
        public ObservableCollection<Airport> GetAirports();
}
