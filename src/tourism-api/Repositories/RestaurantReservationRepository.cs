using Microsoft.Data.Sqlite;
using tourism_api.Domain;

namespace tourism_api.Repositories;

public class RestaurantReservationRepository
{
    private readonly string _connectionString;
    
    public RestaurantReservationRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:SQLiteConnection"];
    }

    public RestaurantReservation GetById(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT rr.Id, rr.RestaurantId, rr.UserId, rr.ReservationDate, rr.MealType, 
                       rr.NumberOfGuests, rr.Status, rr.CreatedAt,
                       r.Name AS RestaurantName
                FROM RestaurantReservations rr
                INNER JOIN Restaurants r ON rr.RestaurantId = r.Id
                INNER JOIN Users u ON rr.UserId = u.Id
                WHERE rr.Id = @Id";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new RestaurantReservation
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    RestaurantId = Convert.ToInt32(reader["RestaurantId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    ReservationDate = DateTime.Parse(reader["ReservationDate"].ToString()),
                    MealType = reader["MealType"].ToString(),
                    NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                    Status = reader["Status"].ToString(),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                    Restaurant = new Restaurant
                    {
                        Id = Convert.ToInt32(reader["RestaurantId"]),
                        Name = reader["RestaurantName"].ToString()
                    }
                };
            }

            return null;
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

    public List<RestaurantReservation> GetByUserId(int userId)
    {
        List<RestaurantReservation> reservations = new List<RestaurantReservation>();

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT rr.Id, rr.RestaurantId, rr.UserId, rr.ReservationDate, rr.MealType, 
                       rr.NumberOfGuests, rr.Status, rr.CreatedAt,
                       r.Name AS RestaurantName
                FROM RestaurantReservations rr
                INNER JOIN Restaurants r ON rr.RestaurantId = r.Id
                INNER JOIN Users u ON rr.UserId = u.Id
                WHERE rr.UserId = @UserId
                AND rr.Status = 'confirmed'
                ORDER BY rr.CreatedAt DESC";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(new RestaurantReservation
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    RestaurantId = Convert.ToInt32(reader["RestaurantId"]),
                    UserId = Convert.ToInt32(reader["UserId"]),
                    ReservationDate = DateTime.Parse(reader["ReservationDate"].ToString()),
                    MealType = reader["MealType"].ToString(),
                    NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                    Status = reader["Status"].ToString(),
                    CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString()),
                    Restaurant = new Restaurant
                    {
                        Id = Convert.ToInt32(reader["RestaurantId"]),
                        Name = reader["RestaurantName"].ToString()
                    }
                });
            }

            return reservations;
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

    public RestaurantReservation Create(RestaurantReservation reservation)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                INSERT INTO RestaurantReservations (RestaurantId, UserId, ReservationDate, MealType, NumberOfGuests, Status, CreatedAt)
                VALUES (@RestaurantId, @UserId, @ReservationDate, @MealType, @NumberOfGuests, @Status, @CreatedAt);
                SELECT last_insert_rowid();";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@RestaurantId", reservation.RestaurantId);
            command.Parameters.AddWithValue("@UserId", reservation.UserId);
            command.Parameters.AddWithValue("@ReservationDate", reservation.ReservationDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@MealType", reservation.MealType);
            command.Parameters.AddWithValue("@NumberOfGuests", reservation.NumberOfGuests);
            command.Parameters.AddWithValue("@Status", reservation.Status);
            command.Parameters.AddWithValue("@CreatedAt", reservation.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

            int id = Convert.ToInt32(command.ExecuteScalar());
            reservation.Id = id;

            return reservation;
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

    public RestaurantReservation CancelReservation(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                UPDATE RestaurantReservations 
                SET Status = 'cancelled'
                WHERE Id = @Id";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return GetById(id);
            }

            return null;
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

    public int GetReservedCapacityForDateAndMeal(int restaurantId, DateTime reservationDate, string mealType)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT IFNULL(SUM(NumberOfGuests), 0) as TotalReserved
                FROM RestaurantReservations 
                WHERE RestaurantId = @RestaurantId 
                AND ReservationDate = @ReservationDate 
                AND MealType = @MealType 
                AND Status IN ('confirmed', 'pending')";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@RestaurantId", restaurantId);
            command.Parameters.AddWithValue("@ReservationDate", reservationDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@MealType", mealType);

            int reservedCapacity = Convert.ToInt32(command.ExecuteScalar());

            return reservedCapacity;
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

    public bool CanCancelReservation(int reservationId)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                SELECT ReservationDate, MealType 
                FROM RestaurantReservations 
                WHERE Id = @Id AND Status = 'confirmed'";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", reservationId);

            using SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                string reservationDateStr = reader["ReservationDate"].ToString();
                if (!DateTime.TryParse(reservationDateStr, out DateTime reservationDate))
                {
                    Console.WriteLine("Nevalidan format datuma.");
                    return false;
                }
                string mealType = reader["MealType"].ToString().ToLower();
                DateTime currentDateTime = DateTime.Now;

                DateTime mealDateTime;

                if (mealType == "dorucak")
                {
                    mealDateTime = reservationDate.Date.AddHours(8);
                }
                else if (mealType == "rucak")
                {
                    mealDateTime = reservationDate.Date.AddHours(13); 
                }
                else if (mealType == "vecera")
                {
                    mealDateTime = reservationDate.Date.AddHours(18);
                }
                else
                {
                    throw new Exception("Nepoznat tip obroka.");
                }

                TimeSpan cancelLimit;

                if (mealType == "dorucak")
                {
                    cancelLimit = TimeSpan.FromHours(12);
                }
                else
                {
                    cancelLimit = TimeSpan.FromHours(4);
                }

                DateTime cancelDeadline = mealDateTime - cancelLimit;

                if (currentDateTime > cancelDeadline)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            return false;
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
} 