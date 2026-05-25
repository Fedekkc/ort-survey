using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
	public class Pregunta
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        private int id { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(300, MinimumLength = 5, ErrorMessage = "La pregunta debe tener entre {2} y {1} caracteres.")]
        private string texto { get; set; }
		[ForeignKey("Encuesta")]
        private int idEncuesta { get; set; }
		private List<Opcion> opciones { get; set; } = new List<Opcion>();
        private List<Respuesta> respuestas { get; set; } = new List<Respuesta>();
    }
}