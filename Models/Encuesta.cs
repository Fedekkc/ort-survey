using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrtSurvey.Models
{
    [Table("Encuesta")]
    public class Encuesta
    {
        [Key]
        private int id { get; set; }
        private string titulo { get; set; }
        private string descripcion { get; set; }
        private bool esPublica { get; set; }
        private int limiteRespuestas { get; set; }
        private DateTime fechaCierre { get; set; }
        private int idUsuario { get; set; }
        private string codigoQR { get; set; }
    }
}
