using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("preguntas")]
public class PreguntaController : ControllerBase
{
    [HttpPost]
    public IActionResult CrearPregunta([FromBody] JsonElement request)
    {
        return Ok(new
        {
            id = "pre-1",
            texto = request.TryGetProperty("texto", out var texto) ? texto.GetString() : null,
            tipo = request.TryGetProperty("tipo", out var tipo) ? tipo.GetString() : null
        });
    }

    [HttpPut("{id}")]
    public IActionResult EditarPregunta(string id, [FromBody] JsonElement request)
    {
        return Ok(new
        {
            id,
            texto = request.TryGetProperty("texto", out var texto) ? texto.GetString() : null,
            tipo = request.TryGetProperty("tipo", out var tipo) ? tipo.GetString() : null
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarPregunta(string id)
    {
        return NoContent();
    }
}
