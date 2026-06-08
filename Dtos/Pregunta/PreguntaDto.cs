namespace OrtSurvey.Dtos.Pregunta;

public class PreguntaDto
{
    //dto de salida

    public int Id { get; init; }
    public string Texto { get; init; } = string.Empty;

    public int IdEncuesta { get; init; }
    
}