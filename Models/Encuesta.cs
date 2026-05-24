using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrtSurvey.Models
{
    [Table("Encuesta")]
    public class Encuesta
    {
        [Key]
        private int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(120, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        private string titulo { get; set; }
        [StringLength(500, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        private string descripcion { get; set; }
        private bool esPublica { get; set; } = true;
        [Range(1, int.MaxValue, ErrorMessage = "El campo {0} debe ser mayor que 0.")]
        private int limiteRespuestas { get; set; } = 1;
        private DateTime fechaCierre { get; set; }
        [ForeignKey("Usuario")]
        private int idUsuario { get; set; }
        private string codigoQR { get; set; }
    }
}
