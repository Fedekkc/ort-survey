using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
    public class Usuario
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        private int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        private string nombre { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
		[EmailAddress(ErrorMessage = "El campo {0} debe ser una dirección de correo electrónico válida.")]
        //falta revisar correo en la base de datos para evitar duplicados
        private string correo { get; set; }
		[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(80, MinimumLength = 8, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^(?=.*\d).{8,80}$", ErrorMessage = "El campo {0} debe tener al menos 8 caracteres y  un número.")]

        private string passwordHash { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(13, 120, ErrorMessage = "El campo {0} debe ser un número entre {1} y {2}.")]
        private int edad { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EnumDataType(typeof(Genero), ErrorMessage = "El campo {0} debe ser un valor válido.")]
        private Genero genero { get; set; }
        //use list xq no puedo usar arraylist
		private List<Encuesta> encuestas { get; set; } = new List<Encuesta>();


    }
}
