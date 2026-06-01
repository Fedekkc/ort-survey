using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("reportes")]
public class ReporteController : ControllerBase
{
    [HttpGet("{idEncuesta}/csv")]
    public IActionResult ExportCsv(string idEncuesta)
    {
        return Content($"encuesta_id,{idEncuesta}", "text/csv");
    }

    [HttpGet("{idEncuesta}/excel")]
    public IActionResult ExportExcel(string idEncuesta)
    {
        return Content($"Excel pendiente para {idEncuesta}");
    }

    [HttpGet("{idEncuesta}/pdf")]
    public IActionResult ExportPdf(string idEncuesta)
    {
        return Content($"PDF pendiente para {idEncuesta}");
    }

    [HttpPost("{idEncuesta}/mail")]
    public IActionResult EnviarMail(string idEncuesta, [FromBody] JsonElement request)
    {
        return Accepted(new
        {
            idEncuesta,
            destinatario = request.TryGetProperty("destinatario", out var destinatario) ? destinatario.GetString() : null,
            asunto = request.TryGetProperty("asunto", out var asunto) ? asunto.GetString() : null
        });
    }
}
