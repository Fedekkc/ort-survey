using Microsoft.Extensions.Logging;
using OrtSurvey.Context;
using OrtSurvey.Models;
using OrtSurvey.Dtos.Encuesta;

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
            var e = _db.Encuestas.FirstOrDefault(x => x.id_encuesta == id);
            if (e == null) return null;
            return MapToDto(e);
        }

        public List<EncuestaDto> GetPublicas()
        {
            var list = _db.Encuestas.Where(x => x.es_publica).ToList();
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
                estado = dto.estado,
                id_usuario = idUsuario,
                fecha_creacion = DateTime.Now
            };

            _db.Encuestas.Add(entity);
            _db.SaveChanges();

            _logger.LogInformation("Encuesta creada id={Id} por usuario={User}", entity.id_encuesta, idUsuario);

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
            return MapToDto(entity);
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
            titulo = e.titulo,
            descripcion = e.descripcion,
            es_publica = e.es_publica,
            fecha_creacion = e.fecha_creacion,
            fecha_cierre = e.fecha_cierre,
            estado = e.estado,
            id_usuario = e.id_usuario
        };
    }
}
