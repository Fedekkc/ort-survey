using System.ComponentModel.DataAnnotations;
using OrtSurvey.Models;
namespace OrtSurvey.Dtos.Pregunta;


public class CreatePreguntaRequest
{
    //dto de entrada, no hace falta el id
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [StringLength(300, MinimumLength = 5, ErrorMessage = "La pregunta debe tener entre {2} y {1} caracteres.")]
    public string Texto { get; init; } = string.Empty;

    public List<CreateOpcionRequest> Opciones { get; init; } = new List<CreateOpcionRequest>();

}