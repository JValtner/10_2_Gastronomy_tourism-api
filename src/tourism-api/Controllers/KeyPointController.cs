using Microsoft.AspNetCore.Mvc;
using tourism_api.Domain;
using tourism_api.Repositories;

namespace tourism_api.Controllers;

[Route("api/key-points")]
[ApiController]
public class KeyPointController : ControllerBase
{
    private readonly TourRepository _tourRepo;
    private readonly KeyPointRepository _keyPointRepo;

    public KeyPointController(IConfiguration configuration)
    {
        _tourRepo = new TourRepository(configuration);
        _keyPointRepo = new KeyPointRepository(configuration);
    }

    [HttpGet]
    public ActionResult<KeyPoints> GetAll()
    {

        try
        {
            List<KeyPoints> keypoints = _keyPointRepo.GetAll();
            if (keypoints.Count > 0)
            {
                return Ok(keypoints);
            }
            else
            {
                return NotFound("No keypoints in the colection");
            }
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while fetching keypoints.");
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Tour> GetById(int id)
    {
        try
        {
            KeyPoints keypoint = _keyPointRepo.GetById(id);
            if (keypoint == null)
            {
                return NotFound($"Keypoint with ID {id} not found.");
            }
            return Ok(keypoint);
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while fetching the keypoint.");
        }
    }

    [HttpPost]
    public ActionResult<KeyPoints> Create([FromBody] KeyPoints newKeyPoint)
    {
        if (!newKeyPoint.IsValid())
        {
            return BadRequest("Invalid key point data.");
        }

        try
        {
            KeyPoints createdKeyPoint = _keyPointRepo.Create(newKeyPoint);
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
            
            bool isDeleted = _keyPointRepo.Delete(id);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound($"Key point with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return Problem("An error occurred while deleting the key point.");
        }
    }
}

