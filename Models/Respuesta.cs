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
		// Identificador de envío (agrupa varias respuestas de un mismo envío)
		public Guid? submission_id { get; set; }

		public int? id_usuario { get; set; }
		public int id_pregunta { get; set; }
		public int? id_opcion { get; set; }

		public string valor_texto { get; set; }

		public string ip_respondedor { get; set; }
		public DateTime fecha_respuesta { get; set; }

		// Navegación
		public Usuario Usuario { get; set; }
		public Pregunta Pregunta { get; set; }
		public Opcion Opcion { get; set; }
	}

}