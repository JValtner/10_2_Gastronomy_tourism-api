using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers
{
    [Route("api/tour-reservations")]
    [ApiController]
    public class TourReservationController : ControllerBase
    {
        private readonly TourReservationRepository _tourReservationRepo;
        private readonly UserRepository _userRepo;
        private readonly TourRepository _tourRepo;
        public TourReservationController(IConfiguration configuration)
        {
            _tourReservationRepo = new TourReservationRepository(configuration);
            _userRepo = new UserRepository(configuration);
            _tourRepo = new TourRepository(configuration);
        }
        [HttpGet("user/{userId}")]
        public ActionResult<List<TourReservations>> GetByUserId(int userId)
        {
            try
            {
                User user = _userRepo.GetById(userId);
                if (user == null)
                {
                    return NotFound($"Korisnik sa ID {userId} nije pronađen.");
                }

                List<TourReservations> reservations = _tourReservationRepo.GetByUserId(userId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return Problem("Greška pri učitavanju rezervacija korisnika.");
            }
        }
        [HttpGet("tour/{tourId}")]
        public ActionResult<List<TourReservations>> GetByTourId(int tourId)
        {
            try
            {
                Tour tour = _tourRepo.GetById(tourId);
                if (tour == null)
                {
                    return NotFound($"Tura sa ID {tourId} nije pronađen.");
                }

                List<TourReservations> reservations = _tourReservationRepo.GetByUserId(tourId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return Problem("Greška pri učitavanju rezervacija korisnika.");
            }
        }

        [HttpPost]
        public ActionResult<TourReservations> Create([FromBody] TourReservations tourReservation)
        {
            if (!tourReservation.IsValid())
            {
                return BadRequest("Invalid Reservation data.");
            }
            //check available space for new reservation
            if (!_tourReservationRepo.CheckAvailableSpace(tourReservation.TourId, tourReservation.NumberOfGuests))
            {
                return Conflict("Tour is fully booked, no available space.");
            }
            
            try
            {
                TourReservations createdTourReservation = _tourReservationRepo.Create(tourReservation);
                return Ok(createdTourReservation);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating the Reservation.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            TourReservations tourReservation = _tourReservationRepo.GetById(id);
            if (!_tourReservationRepo.CheckCancelTime(tourReservation.TourId))
            {
                return Conflict("You cannot cancel the reservation less than 24 hours before the tour starts.");
            }
            try
            {
                bool isDeleted = _tourReservationRepo.Delete(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound($"Reservation with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting the Feedback.");
            }
        }

    }
}
