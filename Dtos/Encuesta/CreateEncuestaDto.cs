using System;
using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos.Encuesta
{
    public class CreateEncuestaDto
    {
        [Required]
        [StringLength(120, MinimumLength = 5)]
        public string titulo { get; set; }

        [StringLength(500)]
        public string descripcion { get; set; }

        public bool? es_publica { get; set; }

        public DateTime? fecha_cierre { get; set; }

        [StringLength(20)]
        public string estado { get; set; }
    }
}
