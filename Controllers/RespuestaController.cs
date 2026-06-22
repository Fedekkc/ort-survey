using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using OrtSurvey.Services.Respuesta;

namespace OrtSurvey.Controllers;

[ApiController]
[Route("respuestas")]
public class RespuestaController : ControllerBase
{
    private readonly RespuestaService _respuestaService;

    public RespuestaController(RespuestaService respuestaService)
    {
        _respuestaService = respuestaService;
    }

    [HttpPost]
    public async Task<IActionResult> ResponderEncuesta([FromBody] JsonElement request)
    {
        try
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _respuestaService.ResponderEncuestaAsync(request, ip);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("encuesta/{id}")]
    public async Task<IActionResult> ObtenerRespuestas(string id)
    {
        if (!int.TryParse(id, out var encuestaId)) return BadRequest("Id de encuesta invalido");

        var list = await _respuestaService.ObtenerPorEncuestaAsync(encuestaId);
        return Ok(list);
    }
}
