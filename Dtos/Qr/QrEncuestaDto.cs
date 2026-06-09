namespace OrtSurvey.Dtos.Qr;

public class QrEncuestaDto
{
    public int id_encuesta { get; set; }
    public string url { get; set; } = string.Empty;
    public string qr_base64 { get; set; } = string.Empty;
    public string content_type { get; set; } = "image/png";
}
