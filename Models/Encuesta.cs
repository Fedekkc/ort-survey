using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrtSurvey.Models
{
    [Table("Encuesta")]
    public class Encuesta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_encuesta { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(120, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string titulo { get; set; }

        [StringLength(500, ErrorMessage = "El campo {0} no puede exceder los {1} caracteres.")]
        public string descripcion { get; set; }

        public bool es_publica { get; set; } = true;

        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_cierre { get; set; }

        public string estado { get; set; }

        public int id_usuario { get; set; }
        public Usuario Usuario { get; set; }

        public List<Pregunta> Preguntas { get; set; } = new List<Pregunta>();
    }
}
