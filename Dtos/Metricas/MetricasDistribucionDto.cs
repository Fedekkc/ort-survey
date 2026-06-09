using System.Collections.Generic;

namespace OrtSurvey.Dtos.Metricas
{
    public class MetricasDistribucionDto
    {
        public int IdPregunta { get; set; }
        public string TextoPregunta { get; set; }
        public int TotalRespuestas { get; set; }
        public List<MetricasOpcionDto> Opciones { get; set; } = new List<MetricasOpcionDto>();
        public int TotalRespuestasTextoLibre { get; set; }
    }
}