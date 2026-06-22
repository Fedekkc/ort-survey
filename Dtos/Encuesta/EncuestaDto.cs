using System;
using System.Collections.Generic;

namespace OrtSurvey.Dtos.Encuesta
{
    public class EncuestaDto
    {
        public int id_encuesta { get; set; }
        public string codigo_publico { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public bool es_publica { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public string estado { get; set; }
        public int id_usuario { get; set; }
        public string creador_nombre { get; set; }
        public List<EncuestaPreguntaDto> preguntas { get; set; } = new();
    }
}
