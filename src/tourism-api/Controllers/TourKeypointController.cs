using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers;

[Route("api/tours/{tourId}/key-points/{keyPointId}")]
[ApiController]
public class TourKeypointController : ControllerBase
{
    private readonly TourRepository _tourRepo;
    private readonly KeyPointRepository _keyPointRepo;

    public TourKeypointController(IConfiguration configuration)
    {
        _tourRepo = new TourRepository(configuration);
        _keyPointRepo = new KeyPointRepository(configuration);
    }

    [HttpPost]
    public ActionResult<KeyPoints> Create(int tourId,int keyPointId)
    {
        if (keyPointId <=0 || tourId <= 0)
        {
            return BadRequest("Invalid key point data.");
        }

        try
        {
            Tour tour = _tourRepo.GetById(tourId);
            KeyPoints keyPoint = _keyPointRepo.GetById(keyPointId);

            if (tour == null)
                return NotFound($"Tour with ID {tourId} not found.");

            if (keyPoint == null)
                return NotFound($"Key point with ID {keyPointId} not found.");

            bool isAssigned = _tourRepo.AddKeypointTour(tourId, keyPointId);
            if (isAssigned)
            {
                return Ok("Keypoint succesfully assigned");
            }
            return NotFound($"Key point with ID {keyPointId}, or Tour with ID {tourId} not found.");
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while creating the key point.");
        }
    }

    [HttpDelete]
    public ActionResult Delete(int keyPointId, int tourId)
    {
        if (keyPointId <= 0 || tourId <= 0)
        {
            return BadRequest("Invalid key point data.");
        }
        try
        {
            Tour tour = _tourRepo.GetById(tourId);
            KeyPoints keyPoint = _keyPointRepo.GetById(keyPointId);

            if (tour == null)
                return NotFound($"Tour with ID {tourId} not found.");

            if (keyPoint == null)
                return NotFound($"Key point with ID {keyPointId} not found.");

            bool isDeleted = _tourRepo.RemoveKeypointTour(tourId, keyPointId);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound($"Key point with ID {keyPointId}, or Tour with ID {tourId} not found.");
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while deleting the key point.");
        }
    }
}
