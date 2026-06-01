using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register([FromBody] JsonElement request)
    {
        return CreatedAtAction(nameof(GetCurrentUser), new { id = "temp-user" }, new
        {
            id = "temp-user",
            email = request.TryGetProperty("email", out var email) ? email.GetString() : null,
            nombre = request.TryGetProperty("nombre", out var nombre) ? nombre.GetString() : null
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] JsonElement request)
    {
        var email = request.TryGetProperty("email", out var emailValue) ? emailValue.GetString() : string.Empty;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "temp-user"),
            new(ClaimTypes.Email, email ?? string.Empty),
            new(ClaimTypes.Name, email ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return Ok(new
        {
            id = "temp-user",
            email,
            nombre = (string?)null
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? string.Empty;

        return Ok(new
        {
            id = userId,
            email,
            nombre = (string?)null
        });
    }
}
