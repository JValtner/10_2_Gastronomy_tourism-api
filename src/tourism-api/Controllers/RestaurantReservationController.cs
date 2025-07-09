using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers;

[Route("api/reservations")]
[ApiController]
public class RestaurantReservationController : ControllerBase
{
    private readonly RestaurantReservationRepository _reservationRepo;
    private readonly RestaurantRepository _restaurantRepo;
    private readonly UserRepository _userRepo;

    public RestaurantReservationController(IConfiguration configuration)
    {
        _reservationRepo = new RestaurantReservationRepository(configuration);
        _restaurantRepo = new RestaurantRepository(configuration);
        _userRepo = new UserRepository(configuration);
    }

    [HttpGet("user/{userId}")]
    public ActionResult<List<RestaurantReservation>> GetByUserId(int userId)
    {
        try
        {
            User user = _userRepo.GetById(userId);
            if (user == null)
            {
                return NotFound($"Korisnik sa ID {userId} nije pronađen.");
            }

            List<RestaurantReservation> reservations = _reservationRepo.GetByUserId(userId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return Problem("Greška pri učitavanju rezervacija korisnika.");
        }
    }

    [HttpPost]
    public ActionResult<RestaurantReservation> Create([FromBody] RestaurantReservation newReservation)
    {
        if (!newReservation.IsValid())
        {
            return BadRequest("Neispravni podaci za rezervaciju.");
        }

        try
        {
            Restaurant restaurant = _restaurantRepo.GetById(newReservation.RestaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restoran sa ID {newReservation.RestaurantId} nije pronađen.");
            }

            User user = _userRepo.GetById(newReservation.UserId);
            if (user == null)
            {
                return NotFound($"Korisnik sa ID {newReservation.UserId} nije pronađen.");
            }

            List<string> validMealTypes = new List<string> { "dorucak", "rucak", "vecera" };
            if (!validMealTypes.Contains(newReservation.MealType.ToLower()))
            {
                return BadRequest($"Neispravan tip obroka. Dozvoljeni tipovi su: {string.Join(", ", validMealTypes)}");
            }

            int reservedCapacity = _reservationRepo.GetReservedCapacityForDateAndMeal(
                newReservation.RestaurantId, 
                newReservation.ReservationDate, 
                newReservation.MealType);

            int availableCapacity = restaurant.Capacity - reservedCapacity;

            if (newReservation.NumberOfGuests > availableCapacity)
            {
                return BadRequest($"Nema dovoljno mesta. Maksimalan broj gostiju koji možete rezervisati za {newReservation.ReservationDate} - {newReservation.MealType} je {availableCapacity}.");
            }

            newReservation.Status = "confirmed";
            newReservation.CreatedAt = DateTime.Now;

            RestaurantReservation createdReservation = _reservationRepo.Create(newReservation);
            return Ok(createdReservation);
        }
        catch (Exception ex)
        {
            return Problem("Greška pri kreiranju rezervacije.");
        }
    }

    [HttpPut("{id}/cancel")]
    public ActionResult<RestaurantReservation> CancelReservation(int id)
    {
        try
        {
            RestaurantReservation existingReservation = _reservationRepo.GetById(id);
            if (existingReservation == null)
            {
                return NotFound($"Rezervacija sa ID {id} nije pronađena.");
            }

            if (!_reservationRepo.CanCancelReservation(id))
            {
                return BadRequest("Rezervacija se ne može otkazati. Doručak se može otkazati najkasnije 12 sati pre termina, a ručak i večera 4 sata pre termina.");
            }

            RestaurantReservation updatedReservation = _reservationRepo.CancelReservation(id);
            return Ok(updatedReservation);
        }
        catch (Exception ex)
        {
            return Problem("Greška pri otkazivanju rezervacije.");
        }
    }
} 