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

		public int id_pregunta { get; set; }

		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[StringLength(300, MinimumLength = 5, ErrorMessage = "La pregunta debe tener entre {2} y {1} caracteres.")]
		public string texto { get; set; }

		[ForeignKey("Encuesta")]
		public int id_encuesta { get; set; }
		public Encuesta Encuesta { get; set; }

		public List<Opcion> Opciones { get; set; } = new List<Opcion>();
		public List<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
	}
}