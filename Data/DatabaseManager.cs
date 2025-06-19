using Microsoft.Data.Sqlite;

namespace ItixoTestWork.Data;

/// <summary>
/// Manages SQLite database connection and weather data work.
/// </summary>
public class DatabaseManager
{
    private readonly string _connectionString;
    private readonly string _dbPath;

    /// <summary>
    /// Initializes database manager and sets up the database connection.
    /// </summary>
    /// <param name="dbPath">Path to the SQLite database file.</param>
    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        _dbPath = dbPath;

        Init();
    }

    /// <summary>
    /// Ensures the database file and required table existance.
    /// Creates database directory if missing.
    /// </summary>
    private void Init()
    {
        string? dbDirectory = Path.GetDirectoryName(_dbPath);

        if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        CREATE TABLE IF NOT EXISTS WeatherJson (
            ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            Timestamp DATETIME NOT NULL,
            Json TEXT NOT NULL
        );
        ";

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Saves JSON string with current timestamp into database.
    /// </summary>
    public void SaveJson(string json)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        INSERT INTO WeatherJson (Timestamp, Json)
        VALUES ($timestamp, $json);
        ";

        command.Parameters.AddWithValue("$timestamp", DateTime.Now);
        command.Parameters.AddWithValue("$json", json);

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Saves a default message indicating data was unavailable.
    /// </summary>
    public void SaveUnavailableMessage()
    {
        SaveJson("Weather station was not available.");
    }

    /// <summary>
    /// Takes the latest JSON entry from the database.
    /// </summary>
    /// <returns>Last stored JSON string or a fallback message.</returns>
    public string GetLastJson()
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
        SELECT Json 
        FROM WeatherJson 
        ORDER BY Timestamp DESC 
        LIMIT 1
        ";

        var json = command.ExecuteScalar() as string;

        if (json == null)
            return "No records found.";

        return json;
    }
}
