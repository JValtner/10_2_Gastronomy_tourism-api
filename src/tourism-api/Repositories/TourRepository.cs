using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using tourism_api.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace tourism_api.Repositories;

public class TourRepository
{
    private readonly string _connectionString;
    public TourRepository(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionString:SQLiteConnection"];
    }
    public List<Tour> GetPaged(int page, int pageSize, string orderBy, string orderDirection)
    {
        List<Tour> tours = new List<Tour>();

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @$"
                    SELECT t.Id, t.Name, t.Description, t.DateTime, t.MaxGuests, t.Status,
                           u.Id AS GuideId, u.Username 
                    FROM Tours t 
                    INNER JOIN Users u ON t.GuideId = u.Id
                    ORDER BY {orderBy} {orderDirection} LIMIT @PageSize OFFSET @Offset";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                tours.Add(new Tour
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    DateTime = Convert.ToDateTime(reader["DateTime"]),
                    MaxGuests = Convert.ToInt32(reader["MaxGuests"]),
                    Status = reader["Status"].ToString(),
                    GuideId = Convert.ToInt32(reader["GuideId"]),
                    Guide = new User
                    {
                        Id = Convert.ToInt32(reader["GuideId"]),
                        Username = reader["Username"].ToString()
                    }
                });
            }

            return tours;
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

    public int CountAll()
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "SELECT COUNT(*) FROM Tours";
            using SqliteCommand command = new SqliteCommand(query, connection);

            return Convert.ToInt32(command.ExecuteScalar());
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
    public int CountAllByGuide(int guideId)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @$"
                    SELECT t.Id, t.Name, t.Description, t.DateTime, t.MaxGuests, t.Status,
                           u.Id AS GuideId, u.Username 
                    FROM Tours t 
                    INNER JOIN Users u ON t.GuideId = u.Id
                    WHERE GuideId =@GuideId";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@GuideId", guideId);
            
            return Convert.ToInt32(command.ExecuteScalar());
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

    public List<Tour> GetByGuide(int guideId, int page, int pageSize, string orderBy, string orderDirection)
    {
        List<Tour> tours = new List<Tour>();

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @$"
            SELECT t.Id, t.Name, t.Description, t.DateTime, t.MaxGuests, t.Status,
                   u.Id AS GuideId, u.Username 
            FROM Tours t 
            INNER JOIN Users u ON t.GuideId = u.Id
            WHERE t.GuideId = @GuideId
            ORDER BY {orderBy} {orderDirection} 
            LIMIT @PageSize OFFSET @Offset";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));
            command.Parameters.AddWithValue("@GuideId", guideId);

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                tours.Add(new Tour
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    DateTime = Convert.ToDateTime(reader["DateTime"]),
                    MaxGuests = Convert.ToInt32(reader["MaxGuests"]),
                    Status = reader["Status"].ToString(),
                    GuideId = Convert.ToInt32(reader["GuideId"]),
                    Guide = new User
                    {
                        Id = Convert.ToInt32(reader["GuideId"]),
                        Username = reader["Username"].ToString()
                    }
                });
            }

            return tours;
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
    public List<Tour> GetAllByGuideAndDate(int guideId, DateTime startDate, DateTime endDate)
    {
        List<Tour> tours = new List<Tour>();

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
            SELECT 
                t.Id, t.Name, t.Description, t.DateTime, t.MaxGuests, t.Status,
                u.Id AS GuideId, u.Username,
                kp.Id AS KeyPointId, kp.OrderPosition, kp.Name AS KeyPointName, kp.Description AS KeyPointDescription,
                kp.ImageUrl AS KeyPointImageUrl, kp.Latitude, kp.Longitude,
                tr.Id AS ReservationId, tr.TourId AS ReservationTourId, tr.UserId AS ReservationUserId,
                tr.NumberOfGuests, tr.CreatedOn,
                tf.Id AS FeedbackId, tf.TourId AS FeedbackTourId, tf.UserId AS FeedbackUserId,
                tf.UserRating, tf.UserComment, tf.PostedOn
            FROM Tours t
            INNER JOIN Users u ON t.GuideId = u.Id
            LEFT JOIN TourKeypoints tkp ON t.Id = tkp.TourId
            LEFT JOIN KeyPoints kp ON kp.Id = tkp.KeypointId
            LEFT JOIN TourFeedbacks tf ON t.Id = tf.TourId
            LEFT JOIN TourReservations tr ON t.Id = tr.TourId
            WHERE t.GuideId = @GuideId
            AND t.DateTime BETWEEN @StartDate AND @EndDate
            ORDER BY t.Id";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@GuideId", guideId);
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

        using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int tourId = Convert.ToInt32(reader["Id"]);

                // Try to find existing tour
                var tour = tours.FirstOrDefault(t => t.Id == tourId);

                if (tour == null)
                {
                    tour = new Tour
                    {
                        Id = tourId,
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        DateTime = Convert.ToDateTime(reader["DateTime"]),
                        MaxGuests = Convert.ToInt32(reader["MaxGuests"]),
                        Status = reader["Status"].ToString(),
                        GuideId = Convert.ToInt32(reader["GuideId"]),
                        Guide = new User
                        {
                            Id = Convert.ToInt32(reader["GuideId"]),
                            Username = reader["Username"].ToString()
                        },
                        TourReservations = new List<TourReservations>(),
                        KeyPoints = new List<KeyPoints>(),
                        TourFeedbacks = new List<TourFeedbacks>()
                    };

                    tours.Add(tour);
                }

                // Add KeyPoint if exists
                if (reader["KeyPointId"] != DBNull.Value)
                {
                    var keyPoint = new KeyPoints
                    {
                        Id = Convert.ToInt32(reader["KeyPointId"]),
                        Order = Convert.ToInt32(reader["OrderPosition"]),
                        Name = reader["KeyPointName"].ToString(),
                        Description = reader["KeyPointDescription"].ToString(),
                        ImageUrl = reader["KeyPointImageUrl"].ToString(),
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"])
                    };

                    if (!tour.KeyPoints.Any(kp => kp.Id == keyPoint.Id))
                        tour.KeyPoints.Add(keyPoint);
                }

                // Add Reservation if exists
                if (reader["ReservationId"] != DBNull.Value)
                {
                    var reservation = new TourReservations
                    {
                        Id = Convert.ToInt32(reader["ReservationId"]),
                        TourId = Convert.ToInt32(reader["ReservationTourId"]),
                        UserId = Convert.ToInt32(reader["ReservationUserId"]),
                        NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                    };

                    if (!tour.TourReservations.Any(r => r.Id == reservation.Id))
                        tour.TourReservations.Add(reservation);
                }

                // Add Feedback if exists
                if (reader["FeedbackId"] != DBNull.Value)
                {
                    var feedback = new TourFeedbacks
                    {
                        Id = Convert.ToInt32(reader["FeedbackId"]),
                        TourId = Convert.ToInt32(reader["FeedbackTourId"]),
                        UserId = Convert.ToInt32(reader["FeedbackUserId"]),
                        UserRating = reader["UserRating"] != DBNull.Value ? Convert.ToInt32(reader["UserRating"]) : null,
                        UserComment = reader["UserComment"]?.ToString(),
                        PostedOn = Convert.ToDateTime(reader["PostedOn"])
                    };

                    if (!tour.TourFeedbacks.Any(f => f.Id == feedback.Id))
                        tour.TourFeedbacks.Add(feedback);
                }
            }

            return tours;
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Greška pri konekciji ili SQL upitu: {ex.Message}");
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
    public Tour GetById(int id)
    {
        Tour tour = null;

        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
            SELECT 
                t.Id, t.Name, t.Description, t.DateTime, t.MaxGuests, t.Status,
                u.Id AS GuideId, u.Username,
                kp.Id AS KeyPointId, kp.OrderPosition, kp.Name AS KeyPointName, kp.Description AS KeyPointDescription,
                kp.ImageUrl AS KeyPointImageUrl, kp.Latitude, kp.Longitude,
                tr.Id AS ReservationId, tr.TourId AS ReservationTourId, tr.UserId AS ReservationUserId,
                tr.NumberOfGuests, tr.CreatedOn,
                tf.Id AS FeedbackId, tf.TourId AS FeedbackTourId, tf.UserId AS FeedbackUserId,
                tf.UserRating, tf.UserComment, tf.PostedOn
            FROM Tours t
            INNER JOIN Users u ON t.GuideId = u.Id
            LEFT JOIN TourKeypoints tkp ON t.Id = tkp.TourId
            LEFT JOIN KeyPoints kp ON kp.Id = tkp.KeypointId
            LEFT JOIN TourFeedbacks tf ON t.Id = tf.TourId
            LEFT JOIN TourReservations tr ON t.Id = tr.TourId
            WHERE t.Id = @Id";

            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (tour == null)
                {
                    tour = new Tour
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        DateTime = Convert.ToDateTime(reader["DateTime"]),
                        MaxGuests = Convert.ToInt32(reader["MaxGuests"]),
                        Status = reader["Status"].ToString(),
                        GuideId = Convert.ToInt32(reader["GuideId"]),
                        Guide = new User
                        {
                            Id = Convert.ToInt32(reader["GuideId"]),
                            Username = reader["Username"].ToString()
                        },
                        KeyPoints = new List<KeyPoints>(),
                        TourReservations = new List<TourReservations>(),
                        TourFeedbacks = new List<TourFeedbacks>()
                    };
                }

                // Add KeyPoint if exists
                if (reader["KeyPointId"] != DBNull.Value)
                {
                    var keyPoint = new KeyPoints
                    {
                        Id = Convert.ToInt32(reader["KeyPointId"]),
                        Order = Convert.ToInt32(reader["OrderPosition"]),
                        Name = reader["KeyPointName"].ToString(),
                        Description = reader["KeyPointDescription"].ToString(),
                        ImageUrl = reader["KeyPointImageUrl"].ToString(),
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"])
                    };

                    if (!tour.KeyPoints.Any(kp => kp.Id == keyPoint.Id))
                        tour.KeyPoints.Add(keyPoint);
                }

                // Add Reservation if exists
                if (reader["ReservationId"] != DBNull.Value)
                {
                    var reservation = new TourReservations
                    {
                        Id = Convert.ToInt32(reader["ReservationId"]),
                        TourId = Convert.ToInt32(reader["ReservationTourId"]),
                        UserId = Convert.ToInt32(reader["ReservationUserId"]),
                        NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                    };

                    if (!tour.TourReservations.Any(r => r.Id == reservation.Id))
                        tour.TourReservations.Add(reservation);
                }

                // Add Feedback if exists
                if (reader["FeedbackId"] != DBNull.Value)
                {
                    var feedback = new TourFeedbacks
                    {
                        Id = Convert.ToInt32(reader["FeedbackId"]),
                        TourId = Convert.ToInt32(reader["FeedbackTourId"]),
                        UserId = Convert.ToInt32(reader["FeedbackUserId"]),
                        UserRating = reader["UserRating"] != DBNull.Value ? Convert.ToInt32(reader["UserRating"]) : null,
                        UserComment = reader["UserComment"]?.ToString(),
                        PostedOn = Convert.ToDateTime(reader["PostedOn"])
                    };

                    if (!tour.TourFeedbacks.Any(f => f.Id == feedback.Id))
                        tour.TourFeedbacks.Add(feedback);
                }
            }

            return tour;
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
    public Tour Create(Tour tour)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                    INSERT INTO Tours (Name, Description, DateTime, MaxGuests, Status, GuideId)
                    VALUES (@Name, @Description, @DateTime, @MaxGuests, @Status, @GuideId);
                    SELECT LAST_INSERT_ROWID();";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Name", tour.Name);
            command.Parameters.AddWithValue("@Description", tour.Description);
            command.Parameters.AddWithValue("@DateTime", tour.DateTime);
            command.Parameters.AddWithValue("@MaxGuests", tour.MaxGuests);
            command.Parameters.AddWithValue("@Status", tour.Status);
            command.Parameters.AddWithValue("@GuideId", tour.GuideId);

            tour.Id = Convert.ToInt32(command.ExecuteScalar());

            return tour;
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

    public Tour Update(Tour tour)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                    UPDATE Tours 
                    SET Name = @Name, Description = @Description, DateTime = @DateTime, 
                        MaxGuests = @MaxGuests, Status = @Status
                    WHERE Id = @Id";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", tour.Id);
            command.Parameters.AddWithValue("@Name", tour.Name);
            command.Parameters.AddWithValue("@Description", tour.Description);
            command.Parameters.AddWithValue("@DateTime", tour.DateTime);
            command.Parameters.AddWithValue("@MaxGuests", tour.MaxGuests);
            command.Parameters.AddWithValue("@Status", tour.Status);

            int affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0 ? tour : null;
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
    public bool Delete(int id)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM Tours WHERE Id = @Id";
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
    public bool AddKeypointTour(int tourId, int keyPointId)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = @"
                            INSERT INTO TourKeypoints (KeyPointId, TourId) VALUES (@KeyPointId,@TourId);
                            UPDATE Tours SET Status=""objavljeno"" where id=@TourId;";//Update tour status only if keypoint is added successfully
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@KeyPointId", keyPointId);
            command.Parameters.AddWithValue("@TourId", tourId);


            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
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
    public bool RemoveKeypointTour(int tourId, int keyPointId)
    {
        try
        {
            using SqliteConnection connection = new SqliteConnection(_connectionString);
            connection.Open();

            string query = "DELETE FROM TourKeypoints WHERE KeyPointId = @KeyPointId AND TourId = @TourId";
            using SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@KeyPointId", keyPointId);
            command.Parameters.AddWithValue("@TourId", tourId);

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
