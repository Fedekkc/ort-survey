using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using OrtSurvey.Dtos.Encuesta;
using OrtSurvey.Services.Services;

namespace OrtSurvey.Controllers;

[ApiController]
[Route("encuestas")]
public class EncuestaController : ControllerBase
{
    private readonly EncuestaService _service;

    public EncuestaController(EncuestaService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost]
    public IActionResult CrearEncuesta([FromBody] CreateEncuestaDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetUserIdFromClaims();
        var created = _service.Create(request, userId);

        return CreatedAtAction(nameof(ObtenerEncuesta), new { id = created.id_encuesta }, created);
    }

    [HttpGet("p/{codigo}")]
    public IActionResult ObtenerEncuestaPorCodigo(string codigo)
    {
        var encuesta = _service.GetByCodigoPublico(codigo);
        if (encuesta == null) return NotFound();
        return Ok(encuesta);
    }

    [HttpGet("{id:int}")]
    public IActionResult ObtenerEncuesta(int id)
    {
        var encuesta = _service.GetById(id);
        if (encuesta == null) return NotFound();
        return Ok(encuesta);
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult EditarEncuesta(int id, [FromBody] UpdateEncuestaDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetUserIdFromClaims();

        var existing = _service.GetById(id);
        if (existing == null) return NotFound();
        if (existing.id_usuario != userId) return Forbid();

        var updated = _service.Update(id, request, userId);
        return Ok(updated);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult EliminarEncuesta(int id)
    {
        var userId = GetUserIdFromClaims();

        var existing = _service.GetById(id);
        if (existing == null) return NotFound();
        if (existing.id_usuario != userId) return Forbid();

        var removed = _service.Delete(id, userId);
        if (!removed) return StatusCode(500);
        return NoContent();
    }

    [HttpGet("publicas")]
    public IActionResult ListarPublicas([FromQuery] int limite = 20)
    {
        if (limite < 1) limite = 1;
        if (limite > 50) limite = 50;

        var list = _service.GetPublicas(limite);
        return Ok(list);
    }

    private int GetUserIdFromClaims()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("id")?.Value;
        if (int.TryParse(idClaim, out var id)) return id;
        return 0;
    }
}
