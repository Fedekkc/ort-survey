using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos.Pregunta;

public class CreateOpcionRequest
{
    [Required(ErrorMessage = "El texto de la opcion es obligatorio.")]
    [StringLength(300, MinimumLength = 1)]
    public string texto { get; init; } = string.Empty;
}
