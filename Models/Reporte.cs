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
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EnumDataType(typeof(FormatoReporte), ErrorMessage = "El campo {0} debe ser un valor válido.")]
        private FormatoReporte formato { get; set; }
        private DateTime creadoEn { get; set; } = DateTime.Now;


    }
}