using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
namespace OrtSurvey.Models

{
	[Table("Opcion")]
    public class Opcion
	{
		[Key]
        private int id { get; set; }
		private string texto { get; set; }
		private int idPregunta { get; set; }


	}
}
