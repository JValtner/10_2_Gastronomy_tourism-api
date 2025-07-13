using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers
{
    [Route("api/tour_feedbacks")]
    [ApiController]
    public class TourFeedbackController : ControllerBase
    {
        private readonly TourFeedbackRepository _tourFeedbackRepo;
        public TourFeedbackController(IConfiguration configuration)
        {
            _tourFeedbackRepo = new TourFeedbackRepository(configuration);
        }
        
        [HttpPost]
        public ActionResult<TourFeedbacks> Create([FromBody] TourFeedbacks tourFeedback)
        {
            if (!tourFeedback.IsValid())
            {
                return BadRequest("Feedback point data.");
            }

            try
            {
                TourFeedbacks createdKeyPoint = _tourFeedbackRepo.Create(tourFeedback);
                return Ok(createdKeyPoint);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while creating the key point.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {

                bool isDeleted = _tourFeedbackRepo.Delete(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound($"Feedback with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting the Feedback.");
            }
        }

    }
}
