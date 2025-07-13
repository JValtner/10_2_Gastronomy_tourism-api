using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers
{
    [Route("api/tour_reservations")]
    [ApiController]
    public class TourReservationController : ControllerBase
    {
        private readonly TourReservationRepository _tourReservationRepo;
        public TourReservationController(IConfiguration configuration)
        {
            _tourReservationRepo = new TourReservationRepository(configuration);
        }

        [HttpPost]
        public ActionResult<TourReservations> Create([FromBody] TourReservations tourReservation)
        {
            if (!tourReservation.IsValid())
            {
                return BadRequest("Invalid Reservation data.");
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
