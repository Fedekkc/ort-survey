using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrtSurvey.Controllers;

[ApiController]
[Authorize]
[Route("usuarios")]
public class UsuarioController : ControllerBase
{
    [HttpGet("perfil")]
    public IActionResult GetPerfil()
    {
        return Ok(new
        {
            id = "temp-user",
            email = "usuario@demo.com",
            nombre = "Usuario",
            apellido = "Demo"
        });
    }

    [HttpPut("perfil")]
    public IActionResult UpdatePerfil([FromBody] JsonElement request)
    {
        return Ok(new
        {
            id = "temp-user",
            email = "usuario@demo.com",
            nombre = request.TryGetProperty("nombre", out var nombre) ? nombre.GetString() : null,
            apellido = request.TryGetProperty("apellido", out var apellido) ? apellido.GetString() : null
        });
    }

    [HttpGet("mis-encuestas")]
    public IActionResult GetMisEncuestas()
    {
        return Ok(new[]
        {
            new { id = "enc-1", titulo = "Encuesta demo", publica = true }
        });
    }
}
