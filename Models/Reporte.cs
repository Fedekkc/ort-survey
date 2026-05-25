using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
    public class Reporte
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_reporte { get; set; }

        public string tipo_reporte { get; set; }
        public DateTime fecha_generacion { get; set; }
        public string correo_destino { get; set; }

        [ForeignKey("Encuesta")]
        public int id_encuesta { get; set; }
        public Encuesta Encuesta { get; set; }
    }
}