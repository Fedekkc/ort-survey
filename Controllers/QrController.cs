using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("qr")]
public class QrController : ControllerBase
{
    [HttpGet("{idEncuesta}")]
    public IActionResult GetQr(string idEncuesta)
    {
        return Ok(new
        {
            idEncuesta,
            qr = $"qr-pendiente-{idEncuesta}"
        });
    }
}