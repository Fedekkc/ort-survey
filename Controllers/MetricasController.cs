using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("metricas")]
public class MetricasController : ControllerBase
{
    [HttpGet("{idEncuesta}")]
    public IActionResult GetMetricas(string idEncuesta)
    {
        return Ok(new { encuestaId = idEncuesta, totalRespuestas = 0, totalVotos = 0 });
    }

    [HttpGet("{idEncuesta}/timeline")]
    public IActionResult GetTimeline(string idEncuesta)
    {
        return Ok(new[]
        {
            new { fecha = DateOnly.FromDateTime(DateTime.UtcNow), totalRespuestas = 0 }
        });
    }

    [HttpGet("{idEncuesta}/distribucion")]
    public IActionResult GetDistribucion(string idEncuesta)
    {
        return Ok(new[]
        {
            new { opcion = "Opcion A", cantidad = 0 }
        });
    }
}
