using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrtSurvey.Models
{
    public class RespuestaEncuesta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_respuesta_encuesta { get; set; }

        public int id_encuesta { get; set; }
        public int? id_usuario { get; set; }

        public string ip_respondedor { get; set; }
        public DateTime fecha_respuesta { get; set; }

        // Navegación
        public Encuesta Encuesta { get; set; }
        public Usuario Usuario { get; set; }

        public List<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    }
}
