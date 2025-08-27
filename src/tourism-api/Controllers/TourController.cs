using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;
using tourism_api.Service;

namespace tourism_api.Controllers;

[Route("api/tours")]
[ApiController]
public class TourController : ControllerBase
{
    private readonly TourRepository _tourRepo;
    private readonly UserRepository _userRepo;
    private readonly tourService _tourService;

    public TourController(IConfiguration configuration)
    {
        _tourRepo = new TourRepository(configuration);
        _userRepo = new UserRepository(configuration);
        _tourService = new tourService(configuration);
    }

    [HttpGet]
    public ActionResult GetPaged([FromQuery] int guideId = 0, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string orderBy = "Name", [FromQuery] string orderDirection = "ASC")
    {
        if (guideId > 0)

        {
            List<Tour> tours = _tourRepo.GetByGuide(guideId, page, pageSize, orderBy, orderDirection);
            int totalCount = _tourRepo.CountAllByGuide(guideId);
            Object result = new
            {
                Data = tours,
                TotalCount = totalCount
            };
            return Ok(result);
        }

        // Validacija za orderBy i orderDirection
        List<string> validOrderByColumns = new List<string> { "Name", "Description", "DateTime", "MaxGuests" }; // Lista dozvoljenih kolona za sortiranje
        if (!validOrderByColumns.Contains(orderBy))
        {
            orderBy = "Name"; // Default vrednost
        }

        List<string> validOrderDirections = new List<string> { "ASC", "DESC" }; // Lista dozvoljenih smerova
        if (!validOrderDirections.Contains(orderDirection))
        {
            orderDirection = "ASC"; // Default vrednost
        }

        try
        {
            List<Tour> tours = _tourRepo.GetPaged(page, pageSize, orderBy, orderDirection);
            int totalCount = _tourRepo.CountAll();
            Object result = new
            {
                Data = tours,
                TotalCount = totalCount
            };
            return Ok(result);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while fetching tours.");
        }
    }

    [HttpGet("stats")]
    public ActionResult GetByGuideAndDate(
        [FromQuery] int guideId = 0,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        if (guideId <= 0)
        {
            return NotFound("Guide ID must be provided.");
        }

        DateTime start = startDate ?? DateTime.Now.AddMonths(-5);
        DateTime end = endDate ?? DateTime.Now.AddMonths(5);

        try
        {
            TourStats tourStats = _tourService.GetByUserAndDateRange(guideId, start, end);
            return Ok(tourStats);
        }
        catch (Exception)
        {
            return Problem("An error occurred while fetching tours.");
        }
    }
    [HttpGet("{id}")]
    public ActionResult<Tour> GetById(int id)
    {
        try
        {
            Tour tour = _tourRepo.GetById(id);
            if (tour == null)
            {
                return NotFound($"Tour with ID {id} not found.");
            }
            return Ok(tour);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while fetching the tour.");
        }
    }

    [HttpPost]
    public ActionResult<Tour> Create([FromBody] Tour newTour)
    {
        if (!newTour.IsValid())
        {
            return BadRequest("Invalid tour data.");
        }

        try
        {
            User user = _userRepo.GetById(newTour.GuideId);
            if (user == null)
            {
                return NotFound($"User with ID {newTour.GuideId} not found.");
            }

            Tour createdTour = _tourRepo.Create(newTour);
            return Ok(createdTour);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while creating the tour.");
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Tour> Update(int id, [FromBody] Tour tour)
    {
        if (!tour.IsValid())
        {
            return BadRequest("Invalid tour data.");
        }

        try
        {
            tour.Id = id;
            Tour updatedTour = _tourRepo.Update(tour);
            if (updatedTour == null)
            {
                return NotFound();
            }
            return Ok(updatedTour);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while updating the tour.");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            bool isDeleted = _tourRepo.Delete(id);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound($"Tour with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while deleting the tour.");
        }
    }

    [HttpPost("{oldTourId}/clone-keypoints/{newTourId}")]
    public ActionResult CloneKeypoints(int oldTourId, int newTourId)
    {
        if (oldTourId <= 0 || newTourId <= 0)
            return BadRequest("Invalid tour IDs.");

        try
        {
            Tour oldTour = _tourRepo.GetById(oldTourId);
            Tour newTour = _tourRepo.GetById(newTourId);

            if (oldTour == null)
                return NotFound($"Old Tour with ID {oldTourId} not found.");
            if (newTour == null)
                return NotFound($"New Tour with ID {newTourId} not found.");

            // Implement cloning logic in repository:
            bool success = _tourService.CloneKeypointsFromOldToNew(oldTourId, newTourId);

            if (success)
                return Ok("Keypoints cloned successfully.");

            return Problem("Failed to clone keypoints.");
        }
        catch (Exception ex)
        {
            return Problem($"An error occurred: {ex.Message}");
        }
    }
}
