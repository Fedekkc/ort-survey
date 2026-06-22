using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrtSurvey.Context;
using OrtSurvey.Models;
using OrtSurvey.Dtos.Encuesta;
using OrtSurvey.Helpers;

namespace OrtSurvey.Services.Services
{
    public class EncuestaService
    {
        private readonly OrtSurveyDataBase _db;
        private readonly ILogger<EncuestaService> _logger;

        public EncuestaService(OrtSurveyDataBase db, ILogger<EncuestaService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public EncuestaDto? GetById(int id)
        {
            var e = _db.Encuestas
                .AsNoTracking()
                .Include(x => x.Preguntas)
                    .ThenInclude(p => p.Opciones)
                .FirstOrDefault(x => x.id_encuesta == id);
            if (e == null) return null;
            return MapToDto(e);
        }

        public EncuestaDto? GetByCodigoPublico(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return null;

            var entity = _db.Encuestas
                .Include(x => x.Preguntas)
                    .ThenInclude(p => p.Opciones)
                .FirstOrDefault(x => x.codigo_publico == codigo);

            if (entity == null) return null;

            EnsureCodigoPublico(entity);
            return MapToDto(entity);
        }

        public List<EncuestaDto> GetPublicas(int limite = 20)
        {
            var list = _db.Encuestas
                .AsNoTracking()
                .Include(x => x.Usuario)
                .Where(x => x.es_publica)
                .OrderByDescending(x => x.fecha_creacion)
                .Take(limite)
                .ToList();

            return list.Select(e =>
            {
                var dto = MapToDto(e);
                dto.creador_nombre = e.Usuario?.nombre;
                return dto;
            }).ToList();
        }

        public List<EncuestaDto> GetByUsuario(int idUsuario)
        {
            var list = _db.Encuestas
                .AsNoTracking()
                .Include(x => x.Preguntas)
                    .ThenInclude(p => p.Opciones)
                .Where(x => x.id_usuario == idUsuario)
                .OrderByDescending(x => x.fecha_creacion)
                .ToList();
            return list.Select(MapToDto).ToList();
        }

        public EncuestaDto Create(CreateEncuestaDto dto, int idUsuario)
        {
            var entity = new OrtSurvey.Models.Encuesta
            {
                titulo = dto.titulo,
                descripcion = dto.descripcion,
                es_publica = dto.es_publica ?? true,
                fecha_cierre = dto.fecha_cierre,
                estado = "activa",
                id_usuario = idUsuario,
                fecha_creacion = DateTime.Now,
                codigo_publico = GenerarCodigoUnico()
            };

            foreach (var preguntaDto in dto.preguntas)
            {
                var pregunta = new OrtSurvey.Models.Pregunta
                {
                    texto = preguntaDto.texto
                };

                foreach (var opcionDto in preguntaDto.opciones ?? Enumerable.Empty<CreateEncuestaOpcionDto>())
                {
                    pregunta.Opciones.Add(new OrtSurvey.Models.Opcion { texto = opcionDto.texto });
                }

                entity.Preguntas.Add(pregunta);
            }

            _db.Encuestas.Add(entity);
            _db.SaveChanges();

            _logger.LogInformation(
                "Encuesta creada id={Id} por usuario={User} con {Preguntas} preguntas",
                entity.id_encuesta,
                idUsuario,
                entity.Preguntas.Count);

            return MapToDto(entity);
        }

        public EncuestaDto? Update(int id, UpdateEncuestaDto dto, int requesterId)
        {
            var entity = _db.Encuestas.FirstOrDefault(x => x.id_encuesta == id);
            if (entity == null) return null;

            if (entity.id_usuario != requesterId)
            {
                _logger.LogWarning("Usuario {User} intentó editar encuesta {Id} sin permisos", requesterId, id);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(dto.titulo)) entity.titulo = dto.titulo;
            if (dto.descripcion != null) entity.descripcion = dto.descripcion;
            if (dto.es_publica.HasValue) entity.es_publica = dto.es_publica.Value;
            if (dto.fecha_cierre.HasValue) entity.fecha_cierre = dto.fecha_cierre;
            if (dto.estado != null) entity.estado = dto.estado;

            _db.SaveChanges();

            _logger.LogInformation("Encuesta actualizada id={Id} por usuario={User}", id, requesterId);
            return GetById(id);
        }

        public bool Delete(int id, int requesterId)
        {
            var entity = _db.Encuestas.FirstOrDefault(x => x.id_encuesta == id);
            if (entity == null) return false;
            if (entity.id_usuario != requesterId)
            {
                _logger.LogWarning("Usuario {User} intentó eliminar encuesta {Id} sin permisos", requesterId, id);
                return false;
            }

            _db.Encuestas.Remove(entity);
            _db.SaveChanges();
            _logger.LogInformation("Encuesta eliminada id={Id} por usuario={User}", id, requesterId);
            return true;
        }

        private static EncuestaDto MapToDto(OrtSurvey.Models.Encuesta e) => new EncuestaDto
        {
            id_encuesta = e.id_encuesta,
            codigo_publico = e.codigo_publico,
            titulo = e.titulo,
            descripcion = e.descripcion,
            es_publica = e.es_publica,
            fecha_creacion = e.fecha_creacion,
            fecha_cierre = e.fecha_cierre,
            estado = e.estado,
            id_usuario = e.id_usuario,
            preguntas = e.Preguntas?
                .OrderBy(p => p.id_pregunta)
                .Select(p => new EncuestaPreguntaDto
                {
                    id_pregunta = p.id_pregunta,
                    texto = p.texto,
                    opciones = p.Opciones?
                        .OrderBy(o => o.id_opcion)
                        .Select(o => new EncuestaOpcionDto
                        {
                            id_opcion = o.id_opcion,
                            texto = o.texto
                        })
                        .ToList() ?? new List<EncuestaOpcionDto>()
                })
                .ToList() ?? new List<EncuestaPreguntaDto>()
        };

        private string GenerarCodigoUnico()
        {
            string codigo;
            do
            {
                codigo = EncuestaCodigoHelper.Generar();
            }
            while (_db.Encuestas.Any(e => e.codigo_publico == codigo));

            return codigo;
        }

        private void EnsureCodigoPublico(OrtSurvey.Models.Encuesta entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.codigo_publico)) return;

            entity.codigo_publico = GenerarCodigoUnico();
            _db.SaveChanges();
        }
    }
}
