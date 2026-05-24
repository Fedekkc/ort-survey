using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
namespace OrtSurvey.Models

{
    public class Opcion
	{
		[Key]
        private int id { get; set; }
		[Required]
		[StringLength(200, MinimumLength = 1, ErrorMessage = "La opcion debe tener entre {2} y {1} caracteres.")]
        private string texto { get; set; }
		[ForeignKey("Pregunta")]
        private int idPregunta { get; set; }


	}
}
