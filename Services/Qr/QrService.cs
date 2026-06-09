using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrtSurvey.Context;
using OrtSurvey.Dtos.Qr;
using QRCoder;

namespace OrtSurvey.Services.Qr;

public class QrService
{
    private readonly OrtSurveyDataBase _db;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<QrService> _logger;

    public QrService(
        OrtSurveyDataBase db,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<QrService> logger)
    {
        _db = db;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<QrEncuestaDto?> GenerarQrEncuestaAsync(int idEncuesta, int requesterId, CancellationToken cancellationToken = default)
    {
        var encuesta = await _db.Encuestas
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.id_encuesta == idEncuesta, cancellationToken);

        if (encuesta == null)
        {
            return null;
        }

        if (encuesta.id_usuario != requesterId)
        {
            return null;
        }

        var url = ConstruirUrlEncuesta(idEncuesta);
        var pngBytes = GenerarImagenQr(url);

        _logger.LogInformation("QR generado para encuesta {EncuestaId}", idEncuesta);

        return new QrEncuestaDto
        {
            id_encuesta = idEncuesta,
            url = url,
            qr_base64 = Convert.ToBase64String(pngBytes)
        };
    }

    public async Task<byte[]?> GenerarImagenQrEncuestaAsync(int idEncuesta, int requesterId, CancellationToken cancellationToken = default)
    {
        var resultado = await GenerarQrEncuestaAsync(idEncuesta, requesterId, cancellationToken);
        if (resultado == null)
        {
            return null;
        }

        return Convert.FromBase64String(resultado.qr_base64);
    }

    public string ConstruirUrlEncuesta(int idEncuesta)
    {
        var baseUrl = _configuration["QrSettings:PublicBaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request != null)
            {
                baseUrl = $"{request.Scheme}://{request.Host}";
            }
            else
            {
                baseUrl = "http://localhost:5228";
            }
        }

        return $"{baseUrl.TrimEnd('/')}/encuestas/{idEncuesta}";
    }

    private static byte[] GenerarImagenQr(string contenido)
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
        using var qr = new PngByteQRCode(data);
        return qr.GetGraphic(pixelsPerModule: 20);
    }
}
