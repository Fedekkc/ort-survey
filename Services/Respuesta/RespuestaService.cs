using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrtSurvey.Context;
using OrtSurvey.Models;

namespace OrtSurvey.Services.Respuesta
{
	public class RespuestaSummaryDto
	{
		public string id { get; set; }
		public int encuestaId { get; set; }
		public DateTimeOffset creadaEn { get; set; }
	}

	public class RespuestaService
	{
		private readonly OrtSurveyDataBase _db;
		private readonly ILogger<RespuestaService> _logger;

		public RespuestaService(OrtSurveyDataBase db, ILogger<RespuestaService> logger)
		{
			_db = db;
			_logger = logger;
		}

		public async Task<RespuestaSummaryDto> ResponderEncuestaAsync(JsonElement request, CancellationToken cancellationToken = default)
		{
			if (request.ValueKind == JsonValueKind.Undefined || request.ValueKind == JsonValueKind.Null)
				throw new ArgumentNullException(nameof(request));

			// extraer encuestaId (puede venir como número o como string)
			int encuestaId = 0;
			if (request.TryGetProperty("encuestaId", out var encProp))
			{
				if (encProp.ValueKind == JsonValueKind.Number && encProp.TryGetInt32(out var n)) encuestaId = n;
				else if (encProp.ValueKind == JsonValueKind.String && int.TryParse(encProp.GetString(), out var p)) encuestaId = p;
			}

			if (encuestaId <= 0) throw new ArgumentException("encuestaId es obligatorio y debe ser un entero positivo", nameof(request));

			var entity = new RespuestaEncuesta
			{
				id_encuesta = encuestaId,
				fecha_respuesta = DateTimeOffset.UtcNow.DateTime
			};

			if (request.TryGetProperty("ip", out var ipProp) && ipProp.ValueKind == JsonValueKind.String)
			{
				entity.ip_respondedor = ipProp.GetString();
			}

			if (request.TryGetProperty("id_usuario", out var userProp))
			{
				if (userProp.ValueKind == JsonValueKind.Number && userProp.TryGetInt32(out var u)) entity.id_usuario = u;
				else if (userProp.ValueKind == JsonValueKind.String && int.TryParse(userProp.GetString(), out var up)) entity.id_usuario = up;
			}

			// agregar respuestas individuales si vienen
			if (request.TryGetProperty("respuestas", out var respuestasProp) && respuestasProp.ValueKind == JsonValueKind.Array)
			{
				foreach (var r in respuestasProp.EnumerateArray())
				{
					var resp = new OrtSurvey.Models.Respuesta();

					if (r.TryGetProperty("id_pregunta", out var pid))
					{
						if (pid.ValueKind == JsonValueKind.Number && pid.TryGetInt32(out var v)) resp.id_pregunta = v;
						else if (pid.ValueKind == JsonValueKind.String && int.TryParse(pid.GetString(), out var vp)) resp.id_pregunta = vp;
					}

					if (r.TryGetProperty("id_opcion", out var oid))
					{
						if (oid.ValueKind == JsonValueKind.Number && oid.TryGetInt32(out var v)) resp.id_opcion = v;
						else if (oid.ValueKind == JsonValueKind.String && int.TryParse(oid.GetString(), out var vp)) resp.id_opcion = vp;
					}

					if (r.TryGetProperty("valor_texto", out var vtxt) && vtxt.ValueKind == JsonValueKind.String)
					{
						resp.valor_texto = vtxt.GetString();
					}

					entity.Respuestas.Add(resp);
				}
			}

			_db.RespuestasEncuestas.Add(entity);
			await _db.SaveChangesAsync(cancellationToken);

			_logger.LogInformation("Respuesta de encuesta creada id={Id} encuesta={Encuesta}", entity.id_respuesta_encuesta, entity.id_encuesta);

			return new RespuestaSummaryDto
			{
				id = $"resp-{entity.id_respuesta_encuesta}",
				encuestaId = entity.id_encuesta,
				creadaEn = new DateTimeOffset(entity.fecha_respuesta)
			};
		}

		public async Task<List<RespuestaSummaryDto>> ObtenerPorEncuestaAsync(int idEncuesta, CancellationToken cancellationToken = default)
		{
			var list = await _db.RespuestasEncuestas
				.AsNoTracking()
				.Where(r => r.id_encuesta == idEncuesta)
				.OrderByDescending(r => r.fecha_respuesta)
				.ToListAsync(cancellationToken);

			return list.Select(e => new RespuestaSummaryDto
			{
				id = $"resp-{e.id_respuesta_encuesta}",
				encuestaId = e.id_encuesta,
				creadaEn = new DateTimeOffset(e.fecha_respuesta)
			}).ToList();
		}
	}
}
