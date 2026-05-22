using System;
using System.ComponentModel.DataAnnotations;
namespace OrtSurvey.Models
{
	public class Respuesta
	{
		[Key]
		private int id { get; set; }
		private int idRespEncuesta { get; set; }
		private int idPregunta { get; set; }
		private string valor { get; set; }
		private int edadVotante { get; set; }
		private Genero generoVotante { get; set; }

	}

}