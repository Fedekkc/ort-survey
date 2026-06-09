using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrtSurvey.Services.Qr;
using OrtSurvey.Services.Services;
using System.Security.Claims;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("qr")]
public class QrController : ControllerBase
{
    private readonly QrService _qrService;
    private readonly EncuestaService _encuestaService;

    public QrController(QrService qrService, EncuestaService encuestaService)
    {
        _qrService = qrService;
        _encuestaService = encuestaService;
    }

    [HttpGet("{idEncuesta}")]
    public async Task<IActionResult> GetQr(int idEncuesta, CancellationToken cancellationToken)
    {
        if (idEncuesta <= 0)
        {
            return BadRequest(new { message = "Id de encuesta invalido." });
        }

        var userId = GetUserIdFromClaims();
        var existing = _encuestaService.GetById(idEncuesta);
        if (existing == null)
        {
            return NotFound();
        }

        if (existing.id_usuario != userId)
        {
            return Forbid();
        }

        var qr = await _qrService.GenerarQrEncuestaAsync(idEncuesta, userId, cancellationToken);
        if (qr == null)
        {
            return NotFound();
        }

        return Ok(qr);
    }

    [HttpGet("{idEncuesta}/imagen")]
    public async Task<IActionResult> GetQrImagen(int idEncuesta, CancellationToken cancellationToken)
    {
        if (idEncuesta <= 0)
        {
            return BadRequest(new { message = "Id de encuesta invalido." });
        }

        var userId = GetUserIdFromClaims();
        var existing = _encuestaService.GetById(idEncuesta);
        if (existing == null)
        {
            return NotFound();
        }

        if (existing.id_usuario != userId)
        {
            return Forbid();
        }

        var png = await _qrService.GenerarImagenQrEncuestaAsync(idEncuesta, userId, cancellationToken);
        if (png == null)
        {
            return NotFound();
        }

        return File(png, "image/png", $"encuesta-{idEncuesta}-qr.png");
    }

    private int GetUserIdFromClaims()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        if (int.TryParse(idClaim, out var id))
        {
            return id;
        }

        return 0;
    }
}
