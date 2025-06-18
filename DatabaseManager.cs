using Microsoft.Data.Sqlite;

namespace ItixoTestWork;

public class DatabaseManager
{
    private readonly string _connectionString;
    private readonly string _dbPath;

    public DatabaseManager(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        _dbPath = dbPath;

        Init();
    }

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

    public void SaveUnavailableMessage()
    {
        SaveJson("Weather station was not available.");
    }
}
