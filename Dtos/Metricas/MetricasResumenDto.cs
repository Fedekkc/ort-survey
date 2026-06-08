using System.Collections.Generic;

namespace OrtSurvey.Dtos.Metricas
{
    public class MetricasResumenDto
    {
        public int IdEncuesta { get; set; }
        public string TituloEncuesta { get; set; }
        public int TotalPreguntas { get; set; }
        public int TotalRespuestasEncuesta { get; set; }
        public int TotalVotos { get; set; }
        public int TotalUsuariosIdentificados { get; set; }
        public int TotalUsuariosAnonimos { get; set; }
        public List<MetricasGeneroDto> DistribucionGenero { get; set; } = new List<MetricasGeneroDto>();
    }
}