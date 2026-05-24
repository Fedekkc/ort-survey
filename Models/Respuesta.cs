using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
	public class Respuesta
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        private int id { get; set; }
		[ForeignKey("Opcion")]
        private int idRespEncuesta { get; set; }
		[ForeignKey("Pregunta")]
        private int idPregunta { get; set; }
        // no me acuerdo que era esto, lo dejo por las dudas
        private string valor { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[Range(13, 120, ErrorMessage = "El campo {0} debe ser un número entre {1} y {2}.")]
        private int edadVotante { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[EnumDataType(typeof(Genero), ErrorMessage = "El campo {0} debe ser un valor válido.")]
        private Genero generoVotante { get; set; }

	}

}