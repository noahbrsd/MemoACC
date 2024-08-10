using System;
using System.Data.SQLite;

namespace MemoPilotes
{
    public static class DatabaseHelper
    {
        

        public static void InitializeDatabase()
        {
            string ConnectionString = "Data Source=MemoPilotes.db;Version=3;";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Pilotes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nom TEXT NOT NULL UNIQUE,
                        Note TEXT
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void AjouterOuMettreAJourPilote(string nom, string note)
        {
            string dbPath = "Data Source=MemoPilotes.db;Version=3;";
            using (var connection = new SQLiteConnection(dbPath))
    {
        connection.Open();

        string sql = @"
            INSERT INTO Pilotes (Nom, Note)
            VALUES (@Nom, @Note)
            ON CONFLICT(Nom) DO UPDATE SET Note = @Note";

        using (var command = new SQLiteCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Nom", nom);
            command.Parameters.AddWithValue("@Note", note);

            command.ExecuteNonQuery();
        }
    }
        }

        public static string ObtenirNotePilote(string nom)
        {
            string ConnectionString = "Data Source=MemoPilotes.db;Version=3;";
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Note FROM Pilotes WHERE Nom = @nom";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nom", nom);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Note"].ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}
