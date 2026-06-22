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
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser una dirección de correo electrónico válida.")]
        public string correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string password_hash { get; set; }

        // Información adicional opcional
        public string genero { get; set; }
        public DateTime fecha_creacion { get; set; }

        // Navegación
        public List<Encuesta> Encuestas { get; set; } = new List<Encuesta>();
    }
}
