using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
namespace OrtSurvey.Models

{
    public class Opcion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int id_opcion { get; set; }

		[Required]
		[StringLength(300, MinimumLength = 1, ErrorMessage = "La opcion debe tener entre {2} y {1} caracteres.")]
		public string texto { get; set; }

		public int id_pregunta { get; set; }
		public Pregunta Pregunta { get; set; }

	}
}
