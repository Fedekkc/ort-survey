using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrtSurvey.Services.Metricas;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("metricas")]
public class MetricasController : ControllerBase
{
    private readonly MetricasService _service;

    public MetricasController(MetricasService service)
    {
        _service = service;
    }

    [HttpGet("{idEncuesta}")]
    public IActionResult GetMetricas(int idEncuesta)
    {
        var metricas = _service.GetMetricas(idEncuesta);
        if (metricas == null)
        {
            return NotFound();
        }

        return Ok(metricas);
    }

    [HttpGet("{idEncuesta}/timeline")]
    public IActionResult GetTimeline(int idEncuesta)
    {
        var timeline = _service.GetTimeline(idEncuesta);
        if (timeline == null)
        {
            return NotFound();
        }

        return Ok(timeline);
    }

    [HttpGet("{idEncuesta}/distribucion")]
    public IActionResult GetDistribucion(int idEncuesta)
    {
        var distribucion = _service.GetDistribucion(idEncuesta);
        if (distribucion == null)
        {
            return NotFound();
        }

        return Ok(distribucion);
    }
}
