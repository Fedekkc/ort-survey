using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos.Encuesta
{
    public class CreateEncuestaDto : IValidatableObject
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

        [Required(ErrorMessage = "Debe incluir al menos una pregunta.")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos una pregunta.")]
        public List<CreateEncuestaPreguntaDto> preguntas { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            for (var i = 0; i < preguntas.Count; i++)
            {
                var pregunta = preguntas[i];
                if (string.IsNullOrWhiteSpace(pregunta.texto) || pregunta.texto.Length < 5)
                {
                    yield return new ValidationResult(
                        "La pregunta debe tener entre 5 y 300 caracteres.",
                        new[] { $"{nameof(preguntas)}[{i}].{nameof(CreateEncuestaPreguntaDto.texto)}" });
                }

                for (var j = 0; j < pregunta.opciones?.Count; j++)
                {
                    var opcion = pregunta.opciones[j];
                    if (string.IsNullOrWhiteSpace(opcion.texto))
                    {
                        yield return new ValidationResult(
                            "El texto de la opcion es obligatorio.",
                            new[] { $"{nameof(preguntas)}[{i}].{nameof(CreateEncuestaPreguntaDto.opciones)}[{j}].{nameof(CreateEncuestaOpcionDto.texto)}" });
                    }
                }
            }
        }
    }
}
