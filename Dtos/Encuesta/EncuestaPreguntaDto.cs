namespace OrtSurvey.Dtos.Encuesta;

public class EncuestaPreguntaDto
{
    public int id_pregunta { get; set; }
    public string texto { get; set; } = string.Empty;
    public List<EncuestaOpcionDto> opciones { get; set; } = new();
}
