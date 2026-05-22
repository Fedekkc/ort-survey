using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OrtSurvey.Models
{
    public class Reporte

    {
        [Key]
        private int id { get; set; }
        [ForeignKey("Encuesta")]
        private int idEncuesta { get; set; }
        private FormatoReporte formato { get; set; }
        private DateTime creadoEn { get; set; } = DateTime.Now;


    }
}