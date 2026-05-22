using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
	public class Pregunta
	{
		[Key]
		private int id { get; set; }
		private string texto { get; set; }
		[ForeignKey("Encuesta")]
        private int idEncuesta { get; set; }
		private List<Opcion> opciones { get; set; }
		private List<Respuesta> respuestas { get; set; }
	}
}