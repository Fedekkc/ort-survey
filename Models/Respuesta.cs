using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
    public class Respuesta
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id_respuesta { get; set; }

		public int id_respuesta_encuesta { get; set; }
		public int id_pregunta { get; set; }
		public int? id_opcion { get; set; }

		public string valor_texto { get; set; }

		// Navegación
		public RespuestaEncuesta RespuestaEncuesta { get; set; }
		public Pregunta Pregunta { get; set; }
		public Opcion Opcion { get; set; }
	}

}