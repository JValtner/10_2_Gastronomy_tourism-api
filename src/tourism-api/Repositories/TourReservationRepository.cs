using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using tourism_api.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace tourism_api.Repositories
{
    public class TourReservationRepository
    {
        private readonly string _connectionString;
        private readonly TourRepository _tourRepo;
        public TourReservationRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString:SQLiteConnection"];
            _tourRepo = new TourRepository(configuration);
        }
        
        public List<TourReservations> GetAll()
        {
            List<TourReservations> tourReservations = new List<TourReservations>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id,TourId,UserId, NumberOfGuests,CreatedOn FROM TourReservations ORDER BY Id ASC;";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tourReservations.Add(new TourReservations
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TourId = Convert.ToInt32(reader["TourId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    });
                }

                return tourReservations;
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
        public TourReservations GetById(int id)
        {
            TourReservations tourReservation = null;
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id,TourId,UserId, NumberOfGuests,CreatedOn FROM TourReservations;";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (tourReservation == null)
                    {
                        tourReservation = new TourReservations
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TourId = Convert.ToInt32(reader["TourId"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                            CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                        }; 
                    }
                }

                if (tourReservation != null)
                {
                return tourReservation;
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
        public List<TourReservations> GetByUserId(int UserId)
        {
            List<TourReservations> tourReservations = new List<TourReservations>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id,TourId,UserId, NumberOfGuests,CreatedOn FROM TourReservations  WHERE Userid= @UserId ORDER BY Id ASC;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", UserId);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tourReservations.Add(new TourReservations
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TourId = Convert.ToInt32(reader["TourId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    });
                }

                return tourReservations;
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
        public List<TourReservations> GetByTourId(int TourId)
        {
            List<TourReservations> tourReservations = new List<TourReservations>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = "SELECT Id,TourId,UserId, NumberOfGuests,CreatedOn FROM TourReservations  WHERE Tourid= @TourId ORDER BY Id ASC;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@TourId", TourId);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tourReservations.Add(new TourReservations
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        TourId = Convert.ToInt32(reader["TourId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        NumberOfGuests = Convert.ToInt32(reader["NumberOfGuests"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"]),
                    });
                }

                return tourReservations;
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
        public TourReservations Create(TourReservations tourReservation)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(_connectionString);
                connection.Open();

                string query = @"
                    INSERT INTO TourReservations (TourId, UserId, NumberOfGuests, CreatedOn)
                    VALUES (@TourId, @UserId, @NumberOfGuests, @CreatedOn);
                    SELECT LAST_INSERT_ROWID();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@TourId", tourReservation.TourId);
                command.Parameters.AddWithValue("@UserId", tourReservation.UserId);
                command.Parameters.AddWithValue("@NumberOfGuests", tourReservation.NumberOfGuests);
                command.Parameters.AddWithValue("@CreatedOn", tourReservation.CreatedOn);

                tourReservation.Id = Convert.ToInt32(command.ExecuteScalar());

                return tourReservation;
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

                string query = "DELETE FROM TourReservations WHERE Id = @Id";
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
        public bool CheckAvailableSpace(int tourId, int guestReservation)
        {
            Tour tour = _tourRepo.GetById(tourId);

            if (tour != null && tour.TourReservations != null)
            {
                int maxGuests = tour.MaxGuests;
                int bookedGuests = 0;

                foreach (var reservation in tour.TourReservations)
                {
                    bookedGuests += reservation.NumberOfGuests;
                }

                return bookedGuests +guestReservation<= maxGuests;
            }

            return false; // tour not found or no reservation data
        }
        public bool CheckCancelTime(int tourId)
        {
            Tour tour = _tourRepo.GetById(tourId);

            if (tour.DateTime > DateTime.Now.AddHours(24) ||tour.DateTime < DateTime.Now)
            {
                return true; // OK to cancel
            }
            return false; // Too late to cancel
        }

    }
}



