using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PrivateController : ControllerBase
{
    // 🔒 Solo usuarios con el rol "Root"
    [Authorize(Policy = "Root")]
    [HttpGet("admin-only")]
    public IActionResult GetAdminData()
    {
        return Ok(new { message = "Acceso solo para ROOT." });
    }

    // 🔒 Usuarios con permisos de "read:any_user"
    [Authorize(Policy = "read:any_user")]
    [HttpGet("users")]
    public IActionResult GetUsers()
    {
        return Ok(new { message = "Usuarios obtenidos correctamente." });
    }
}
