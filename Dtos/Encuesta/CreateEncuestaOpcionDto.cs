using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos.Encuesta;

public class CreateEncuestaOpcionDto
{
    [Required(ErrorMessage = "El texto de la opcion es obligatorio.")]
    [StringLength(300, MinimumLength = 1)]
    public string texto { get; set; } = string.Empty;
}
