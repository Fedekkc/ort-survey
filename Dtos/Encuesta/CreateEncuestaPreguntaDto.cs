using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos.Encuesta;

public class CreateEncuestaPreguntaDto
{
    [Required(ErrorMessage = "El texto de la pregunta es obligatorio.")]
    [StringLength(300, MinimumLength = 5)]
    public string texto { get; set; } = string.Empty;

    public List<CreateEncuestaOpcionDto> opciones { get; set; } = new();
}
