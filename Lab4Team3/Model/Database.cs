using System.Text.Json;
using System.IO;
using System.Collections.ObjectModel;
using Npgsql;


namespace Lab2Maui.AirportModel;
// This namespace represents the implementation for a database. It has an interface and a
// database class that implements it.


// This database implements the IDatabase interface.
public class Database : IDatabase
{
    private String connString = GetConnectionString();
    private ObservableCollection<Airport> airports;

    // Constructor that instantiates the list of airports
    public Database() {
        airports = new ObservableCollection<Airport>();
        connString = GetConnectionString();
    }

    // Builds a ConnectionString, which is used to connect to the database
    static String GetConnectionString()
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder();

        connStringBuilder.Host = "heebs-cluster-1971.jxf.gcp-us-central1.cockroachlabs.cloud";
 //"stormy-ocelot-12775.5xj.cockroachlabs.cloudheavy-civet-13392.5xj.gcp-us-central1.cockroachlabs.cloud";
        connStringBuilder.Port = 26257;
        connStringBuilder.SslMode = SslMode.Require;
        connStringBuilder.Username = "hiba"; // won't hardcode this in your app
        connStringBuilder.Password = "gOer8S9X2d7q2HEfMCI64Q"; //"W_QpuQhPPg2fsKXEjir7Iw";
        connStringBuilder.Database = "defaultdb";
        connStringBuilder.ApplicationName = "whatever"; // ignored, apparently
        connStringBuilder.IncludeErrorDetail = true;
        
        return connStringBuilder.ConnectionString;
    }

    // Schema for creating the table
    //     CREATE table airports (
    //          id VARCHAR(4) PRIMARY KEY,
    //          city VARCHAR(200),
    //          date DATE,
    //          rating INT
    //      );

    // public property for airports
    public ObservableCollection<Airport> Airports {get {return airports;} set{}}

    // Reads from a file and saves all the airports into the airports field
    public ObservableCollection<Airport> SelectAllAirports() {
        airports.Clear();
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        
        using var cmd = new NpgsqlCommand("SELECT id, city, date, rating FROM airports", conn);
        using var reader = cmd.ExecuteReader(); // used for SELECT statement, returns a forward-only traversable object
        while (reader.Read()) // each time through we get another row in the table (i.e., another airport)
        {
            String id = reader.GetString(0);
            String city = reader.GetString(1);
            DateTime date = reader.GetDateTime(2);
            Int32 rating = reader.GetInt32(3);
            Airport airport = new(id, city, date, rating);
            airports.Add(airport);
            Console.WriteLine(airport);
        }
        return airports;
    }



    // Selects an airport depending on the id that is passed in
    public Airport? SelectAirport(String id) {
        // For each airport in airports, check if the id matches
        // foreach (Airport airport in airports) {
        //     if (airport.Id == id) {
        //         return airport; // if the id matches, return it
        //     }
        // }
        // return null;

        try 
        {
            // Connection to database
            var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Select the airport according to the id
            using var cmd = new NpgsqlCommand("SELECT id, city, date, rating FROM airports WHERE id = @airportId", conn);
            cmd.Parameters.AddWithValue("airportId", id);
            using var reader = cmd.ExecuteReader();
            Airport thisAirport = new Airport();

            // While reading the table, save the variables and store it into this airport
            while(reader.Read())
            {
                String airportId = reader.GetString(0);
                String city = reader.GetString(1);
                DateTime date = reader.GetDateTime(2);
                Int32 rating = reader.GetInt32(3);
                thisAirport = new Airport(airportId, city, date, rating);
            }

            return thisAirport;

        } catch
        {
            Console.WriteLine("Couldn't select airport");
            return null;
        }
    }

    // Inserts an airport into the airports field
    public bool InsertAirport(Airport airport) {
        // airports.Add(airport); // Insert the airport
        // if (airports.Contains(airport)) { // If succesfully there
        //     // Write it to the file 
        //     UpdateFile();
        //     return true;
        // }
        // return false;

        try
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            // Make a connection
            var cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // Insert into the table
            cmd.CommandText = "INSERT INTO airports VALUES(@id, @city, @date, @rating)";
            cmd.Parameters.AddWithValue("id", airport.Id);
            cmd.Parameters.AddWithValue("city", airport.City);
            cmd.Parameters.AddWithValue("date", airport.DateVisited);
            cmd.Parameters.AddWithValue("rating", airport.Rating);
            cmd.ExecuteNonQuery();
            return true;
        } catch (Npgsql.PostgresException pe)
        {
            Console.WriteLine("Couldn't insert airport: , {0}", pe);
            return false;
        }
    }



    // Deletes an airport depending on the id passed in
    public bool DeleteAirport(String id) {
        // For each airport in the airports field, check the id
        // foreach (Airport airport in airports) {
        //     if (airport.Id == id) { // if the id matches
        //         airports.Remove(airport); // remove it and update the file
        //         UpdateFile();
        //         return true;
        //     }
        // }
        // return false;

        // Make connection
        var conn = new NpgsqlConnection(connString);
        conn.Open();
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        // DELETE statement
        cmd.CommandText = "DELETE FROM airports WHERE id = @airportId";
        cmd.Parameters.AddWithValue("airportId", id);
        int numDeleted = cmd.ExecuteNonQuery();

        // If there was a row deleted return true
        if (numDeleted > 0) 
        {
            return true;
        }
        return false;
    }



    // Updates the file and airports field depending on the airport that is passed in
    public bool UpdateAirport(Airport airport) {
        // Get the current airport
        Airport currentAirport = SelectAirport(airport.Id);
        bool updated = false;

        if (currentAirport != null) { // If the current airport exists
            DeleteAirport(currentAirport.Id); // delete the current contents of it
            InsertAirport(airport); // insert the new contents
            updated = true;
            //UpdateFile(); // update the file
        } else {
            throw new Exception("An error occurred."); // Couldn't update airports
        }
        return updated;
    }

    // Updates the file with the current airports from the field
    private void UpdateFile() {
        String fileName = "database.json";
        // Serialize the airports list
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        String jsonAirports = JsonSerializer.Serialize(airports, options);
        try
        {
            File.WriteAllText(fileName, jsonAirports); // Write to the file
        }
        catch (IOException ex)
        {
            // Handle the exception (e.g., log it or show an error message)
            Console.WriteLine($"Error writing file: {ex.Message}");
        }
    }
}