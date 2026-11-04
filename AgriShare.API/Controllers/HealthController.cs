using Microsoft.AspNetCore.Mvc;

namespace AgriShare.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            Status = "Online",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow
        });
    }
}
