using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrtSurvey.Services.Auth;
using OrtSurvey.Services.Services;
using System.Security.Claims;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly EncuestaService _encuestaService;
    private readonly IAuthService _authService;

    public UsuarioController(EncuestaService encuestaService, IAuthService authService)
    {
        _encuestaService = encuestaService;
        _authService = authService;
    }

    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil(CancellationToken cancellationToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0) return Unauthorized();

        var user = await _authService.GetUserAsync(userId, cancellationToken);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpGet("mis-encuestas")]
    public IActionResult GetMisEncuestas()
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0) return Unauthorized();

        var list = _encuestaService.GetByUsuario(userId);
        return Ok(list);
    }

    private int GetUserIdFromClaims()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        if (int.TryParse(idClaim, out var id)) return id;
        return 0;
    }
}
