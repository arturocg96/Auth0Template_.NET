using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PublicController : ControllerBase
{
    [HttpGet]
    public IActionResult Public()
    {
        return Ok(new
        {
            Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
        });
    }
}