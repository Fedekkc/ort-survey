using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Route("respuestas")]
public class RespuestaController : ControllerBase
{
    [HttpPost]
    public IActionResult ResponderEncuesta([FromBody] JsonElement request)
    {
        return Ok(new
        {
            id = "resp-1",
            encuestaId = request.TryGetProperty("encuestaId", out var encuestaId) ? encuestaId.GetString() : null,
            creadaEn = DateTimeOffset.UtcNow
        });
    }

    [Authorize]
    [HttpGet("encuesta/{id}")]
    public IActionResult ObtenerRespuestas(string id)
    {
        return Ok(new[]
        {
            new { id = "resp-1", encuestaId = id, creadaEn = DateTimeOffset.UtcNow }
        });
    }
}
