using Microsoft.Data.Sqlite;
using tourism_api.Domain;

namespace tourism_api.Repositories;

public class KeyPointRepository
{
    private readonly string _connectionString;

    public KeyPointRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:SQLiteConnection"];
    }

    public List<KeyPoint> GetAll()
    {
        List<KeyPoint> keypoints = new List<KeyPoint>();

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, OrderPosition, Name, Description, ImageUrl, Latitude, Longitude FROM KeyPoints ORDER BY Id ASC;";
            using SqliteCommand command = new SqliteCommand(query, connection);
           
            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                keypoints.Add(new KeyPoint
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Order = Convert.ToInt32(reader["OrderPosition"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    Latitude = Convert.ToDouble(reader["Latitude"]),
                    Longitude = Convert.ToDouble(reader["Longitude"]),

                });
            }

            return keypoints;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            throw;
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
    public KeyPoint GetById(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "SELECT Id, OrderPosition, Name, Description, ImageUrl, Latitude, Longitude FROM KeyPoints WHERE Id = @Id;";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new KeyPoint
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Order = Convert.ToInt32(reader["OrderPosition"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    Latitude = Convert.ToDouble(reader["Latitude"]),
                    Longitude = Convert.ToDouble(reader["Longitude"])
                };
            }

            return null; // Not found
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri konekciji ili SQL izvršenju: {ex.Message}");
            throw;
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }

    public KeyPoint Create(KeyPoint keyPoint)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                    INSERT INTO KeyPoints (OrderPosition, Name, Description, ImageUrl, Latitude, Longitude)
                    VALUES (@Order, @Name, @Description, @ImageUrl, @Latitude, @Longitude);
                    SELECT LAST_INSERT_ROWID();";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Order", keyPoint.Order);
            command.Parameters.AddWithValue("@Name", keyPoint.Name);
            command.Parameters.AddWithValue("@Description", keyPoint.Description);
            command.Parameters.AddWithValue("@ImageUrl", keyPoint.ImageUrl);
            command.Parameters.AddWithValue("@Latitude", keyPoint.Latitude);
            command.Parameters.AddWithValue("@Longitude", keyPoint.Longitude);

            keyPoint.Id = Convert.ToInt32(command.ExecuteScalar());

            return keyPoint;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            throw;
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM KeyPoints WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Neočekivana greška: {ex.Message}");
            throw;
        }
    }
}
