using System;

namespace OrtSurvey.Dtos.Metricas
{
    public class MetricasTimelineDto
    {
        public DateOnly Fecha { get; set; }
        public int TotalRespuestas { get; set; }
    }
}