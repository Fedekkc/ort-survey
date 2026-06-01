using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Route("encuestas")]
public class EncuestaController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public IActionResult CrearEncuesta([FromBody] JsonElement request)
    {
        return CreatedAtAction(nameof(ObtenerEncuesta), new { id = "enc-1" }, new
        {
            id = "enc-1",
            titulo = request.TryGetProperty("titulo", out var titulo) ? titulo.GetString() : null,
            descripcion = request.TryGetProperty("descripcion", out var descripcion) ? descripcion.GetString() : null,
            publica = false,
            cerrada = false
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerEncuesta(string id)
    {
        return Ok(new
        {
            id,
            titulo = "Encuesta demo",
            descripcion = "Descripción demo",
            publica = true,
            cerrada = false
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult EditarEncuesta(string id, [FromBody] JsonElement request)
    {
        return Ok(new
        {
            id,
            titulo = request.TryGetProperty("titulo", out var titulo) ? titulo.GetString() : null,
            descripcion = request.TryGetProperty("descripcion", out var descripcion) ? descripcion.GetString() : null,
            publica = request.TryGetProperty("publica", out var publica) && publica.GetBoolean(),
            cerrada = request.TryGetProperty("cerrada", out var cerrada) && cerrada.GetBoolean()
        });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public IActionResult EliminarEncuesta(string id)
    {
        return NoContent();
    }

    [HttpGet("publicas")]
    public IActionResult ListarPublicas()
    {
        return Ok(new[]
        {
            new { id = "enc-1", titulo = "Encuesta demo", publica = true }
        });
    }
}
