using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{

	[Table("Usuario")]
    public class Usuario
	{
		[Key]
		private int id { get; set; }
		private string nombre { get; set; }
		private string correo { get; set; }
		private string passwordHash { get; set; }
		private int edad { get; set; }
		private Genero genero { get; set; }
		private List<Encuesta> encuestas { get; set; }


	}
}
