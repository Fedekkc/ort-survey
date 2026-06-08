#nullable enable

using OrtSurvey.Context;
using OrtSurvey.Dtos.Metricas;

namespace OrtSurvey.Services.Metricas
{
    public class MetricasService
    {
        private readonly OrtSurveyDataBase _db;

        public MetricasService(OrtSurveyDataBase db)
        {
            _db = db;
        }

        public MetricasResumenDto? GetMetricas(int idEncuesta)
        {
            var encuesta = _db.Encuestas.FirstOrDefault(x => x.id_encuesta == idEncuesta);
            if (encuesta == null)
            {
                return null;
            }

            var preguntas = _db.Preguntas.Where(p => p.id_encuesta == idEncuesta).ToList();
            var respuestasEncuesta = _db.RespuestasEncuestas.Where(r => r.id_encuesta == idEncuesta).ToList();
            var respuestas = ObtenerRespuestas(idEncuesta);

            var usuarios = respuestasEncuesta
                .Where(r => r.id_usuario.HasValue)
                .Join(_db.Usuarios, r => r.id_usuario!.Value, u => u.id_usuario, (_, user) => user)
                .ToList();

            return new MetricasResumenDto
            {
                IdEncuesta = encuesta.id_encuesta,
                TituloEncuesta = encuesta.titulo,
                TotalPreguntas = preguntas.Count,
                TotalRespuestasEncuesta = respuestasEncuesta.Count,
                TotalVotos = respuestas.Count,
                TotalUsuariosIdentificados = usuarios.Count,
                TotalUsuariosAnonimos = respuestasEncuesta.Count - usuarios.Count,
                DistribucionGenero = usuarios
                    .GroupBy(u => string.IsNullOrWhiteSpace(u.genero) ? "NO_INFORMADO" : u.genero.Trim())
                    .Select(g => new MetricasGeneroDto
                    {
                        Genero = g.Key,
                        Cantidad = g.Count()
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .ThenBy(x => x.Genero)
                    .ToList()
            };
        }

        public List<MetricasTimelineDto>? GetTimeline(int idEncuesta)
        {
            var encuestaExists = _db.Encuestas.Any(x => x.id_encuesta == idEncuesta);
            if (!encuestaExists)
            {
                return null;
            }

            return _db.RespuestasEncuestas
                .Where(r => r.id_encuesta == idEncuesta)
                .AsEnumerable()
                .GroupBy(r => DateOnly.FromDateTime(r.fecha_respuesta.Date))
                .Select(g => new MetricasTimelineDto
                {
                    Fecha = g.Key,
                    TotalRespuestas = g.Count()
                })
                .OrderBy(x => x.Fecha)
                .ToList();
        }

        public List<MetricasDistribucionDto>? GetDistribucion(int idEncuesta)
        {
            var encuestaExists = _db.Encuestas.Any(x => x.id_encuesta == idEncuesta);
            if (!encuestaExists)
            {
                return null;
            }

            var preguntas = _db.Preguntas
                .Where(p => p.id_encuesta == idEncuesta)
                .ToList();

            var preguntaIds = preguntas.Select(p => p.id_pregunta).ToList();
            var respuestas = ObtenerRespuestas(idEncuesta)
                .Where(r => preguntaIds.Contains(r.id_pregunta))
                .ToList();

            var opciones = _db.Opciones
                .Where(o => preguntaIds.Contains(o.id_pregunta))
                .ToList();

            var distribucion = new List<MetricasDistribucionDto>();

            foreach (var pregunta in preguntas)
            {
                var respuestasPregunta = respuestas.Where(r => r.id_pregunta == pregunta.id_pregunta).ToList();
                var opcionesPregunta = opciones.Where(o => o.id_pregunta == pregunta.id_pregunta).ToList();

                var totalRespuestas = respuestasPregunta.Count;
                var opcionesDto = opcionesPregunta
                    .GroupJoin(
                        respuestasPregunta.Where(r => r.id_opcion.HasValue),
                        opcion => opcion.id_opcion,
                        respuesta => respuesta.id_opcion!.Value,
                        (opcion, respuestasOpcion) => new MetricasOpcionDto
                        {
                            IdOpcion = opcion.id_opcion,
                            TextoOpcion = opcion.texto,
                            Cantidad = respuestasOpcion.Count(),
                            Porcentaje = totalRespuestas == 0 ? 0 : Math.Round((decimal)respuestasOpcion.Count() * 100m / totalRespuestas, 2)
                        })
                    .OrderByDescending(x => x.Cantidad)
                    .ThenBy(x => x.TextoOpcion)
                    .ToList();

                distribucion.Add(new MetricasDistribucionDto
                {
                    IdPregunta = pregunta.id_pregunta,
                    TextoPregunta = pregunta.texto,
                    TipoPregunta = pregunta.tipo_pregunta,
                    TotalRespuestas = totalRespuestas,
                    Opciones = opcionesDto,
                    TotalRespuestasTextoLibre = respuestasPregunta.Count(r => !r.id_opcion.HasValue && !string.IsNullOrWhiteSpace(r.valor_texto))
                });
            }

            return distribucion;
        }

        private List<OrtSurvey.Models.Respuesta> ObtenerRespuestas(int idEncuesta)
        {
            var respuestaEncuestaIds = _db.RespuestasEncuestas
                .Where(r => r.id_encuesta == idEncuesta)
                .Select(r => r.id_respuesta_encuesta)
                .ToList();

            if (respuestaEncuestaIds.Count == 0)
            {
                return new List<OrtSurvey.Models.Respuesta>();
            }

            return _db.Respuestas
                .Where(r => respuestaEncuestaIds.Contains(r.id_respuesta_encuesta))
                .ToList();
        }
    }
}