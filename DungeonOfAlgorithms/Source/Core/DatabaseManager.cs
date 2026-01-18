using System;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.Xna.Framework;

namespace DungeonOfAlgorithms.Source.Core;

public class DatabaseManager
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance => _instance ??= new DatabaseManager();

    private string _connectionString;

    private DatabaseManager() 
    {
        // Create "Data" folder next to the executable
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "game.db");
        // Directory.CreateDirectory(Path.GetDirectoryName(dbPath)); // BaseDirectory usually exists
        _connectionString = $"Data Source={dbPath}";
        System.Console.WriteLine("DEBUG: Database saved at: " + dbPath);
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var con = new SqliteConnection(_connectionString);
        con.Open();

        string sql = @"
            CREATE TABLE IF NOT EXISTS SaveSlots (
                Id INTEGER PRIMARY KEY,
                Level INTEGER,
                PlayerX REAL,
                PlayerY REAL,
                Score INTEGER
            )";

        using var cmd = new SqliteCommand(sql, con);
        cmd.ExecuteNonQuery();
    }

    public void SaveGame(int level, Vector2 position, int score)
    {
        using var con = new SqliteConnection(_connectionString);
        con.Open();

        // Simple UPSERT or Replace logic for Slot 1
        string sql = "INSERT OR REPLACE INTO SaveSlots (Id, Level, PlayerX, PlayerY, Score) VALUES (1, @Level, @X, @Y, @Score)";
        
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@Level", level);
        cmd.Parameters.AddWithValue("@X", position.X);
        cmd.Parameters.AddWithValue("@Y", position.Y);
        cmd.Parameters.AddWithValue("@Score", score);
        
        cmd.ExecuteNonQuery();
        System.Diagnostics.Debug.WriteLine("Game Saved!");
    }

    public (int Level, Vector2 Position, int Score)? LoadGame()
    {
        using var con = new SqliteConnection(_connectionString);
        con.Open();

        string sql = "SELECT Level, PlayerX, PlayerY, Score FROM SaveSlots WHERE Id = 1";
        using var cmd = new SqliteCommand(sql, con);
        
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            int level = reader.GetInt32(0);
            float x = reader.GetFloat(1);
            float y = reader.GetFloat(2);
            int score = reader.GetInt32(3);
            return (level, new Vector2(x, y), score);
        }
        
        return null;
    }
}

