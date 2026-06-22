using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrtSurvey.Context;

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

		public async Task<RespuestaSummaryDto> ResponderEncuestaAsync(JsonElement request, string? ipCliente, CancellationToken cancellationToken = default)
		{
			if (request.ValueKind == JsonValueKind.Undefined || request.ValueKind == JsonValueKind.Null)
				throw new ArgumentNullException(nameof(request));

			int encuestaId = 0;
			if (request.TryGetProperty("encuestaId", out var encProp))
			{
				if (encProp.ValueKind == JsonValueKind.Number && encProp.TryGetInt32(out var n)) encuestaId = n;
				else if (encProp.ValueKind == JsonValueKind.String && int.TryParse(encProp.GetString(), out var p)) encuestaId = p;
			}

			if (encuestaId <= 0) throw new ArgumentException("encuestaId es obligatorio y debe ser un entero positivo", nameof(request));

			var encuestaExiste = await _db.Encuestas.AnyAsync(e => e.id_encuesta == encuestaId, cancellationToken);
			if (!encuestaExiste) throw new ArgumentException("La encuesta no existe", nameof(request));

			if (!string.IsNullOrWhiteSpace(ipCliente))
			{
				var ipYaVoto = await _db.Respuestas
					.AsNoTracking()
					.AnyAsync(r =>
						r.ip_respondedor == ipCliente &&
						r.Pregunta.id_encuesta == encuestaId,
						cancellationToken);

				if (ipYaVoto)
				{
					throw new InvalidOperationException("Ya respondiste esta encuesta. Solo se permite un voto por IP.");
				}
			}

			int? idUsuario = null;
			if (request.TryGetProperty("id_usuario", out var userProp))
			{
				if (userProp.ValueKind == JsonValueKind.Number && userProp.TryGetInt32(out var u)) idUsuario = u;
				else if (userProp.ValueKind == JsonValueKind.String && int.TryParse(userProp.GetString(), out var up)) idUsuario = up;
			}

			if (!request.TryGetProperty("respuestas", out var respuestasProp) || respuestasProp.ValueKind != JsonValueKind.Array)
			{
				throw new ArgumentException("El campo respuestas es obligatorio y debe ser un array", nameof(request));
			}

			var submissionId = Guid.NewGuid();
			var ahora = DateTime.UtcNow;
			var entities = new List<OrtSurvey.Models.Respuesta>();

			foreach (var r in respuestasProp.EnumerateArray())
			{
				var resp = new OrtSurvey.Models.Respuesta
				{
					submission_id = submissionId,
					id_usuario = idUsuario,
					ip_respondedor = ipCliente,
					fecha_respuesta = ahora
				};

				if (r.TryGetProperty("id_pregunta", out var pid))
				{
					if (pid.ValueKind == JsonValueKind.Number && pid.TryGetInt32(out var v)) resp.id_pregunta = v;
					else if (pid.ValueKind == JsonValueKind.String && int.TryParse(pid.GetString(), out var vp)) resp.id_pregunta = vp;
				}

				if (resp.id_pregunta <= 0)
				{
					throw new ArgumentException("Cada respuesta debe incluir id_pregunta válido", nameof(request));
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

				entities.Add(resp);
			}

			if (entities.Count == 0)
			{
				throw new ArgumentException("Debe enviar al menos una respuesta", nameof(request));
			}

			var idsPregunta = entities.Select(x => x.id_pregunta).Distinct().ToList();
			var preguntasValidas = await _db.Preguntas
				.Where(p => p.id_encuesta == encuestaId && idsPregunta.Contains(p.id_pregunta))
				.Select(p => p.id_pregunta)
				.ToListAsync(cancellationToken);

			if (preguntasValidas.Count != idsPregunta.Count)
			{
				throw new ArgumentException("Hay preguntas que no pertenecen a la encuesta", nameof(request));
			}

			_db.Respuestas.AddRange(entities);
			await _db.SaveChangesAsync(cancellationToken);

			_logger.LogInformation("Respuesta de encuesta creada submission={Submission} encuesta={Encuesta} cantidad={Cantidad}", submissionId, encuestaId, entities.Count);

			return new RespuestaSummaryDto
			{
				id = $"sub-{submissionId}",
				encuestaId = encuestaId,
				creadaEn = new DateTimeOffset(ahora)
			};
		}

		public async Task<List<RespuestaSummaryDto>> ObtenerPorEncuestaAsync(int idEncuesta, CancellationToken cancellationToken = default)
		{
			var list = await _db.Respuestas
				.AsNoTracking()
				.Where(r => r.Pregunta.id_encuesta == idEncuesta)
				.OrderByDescending(r => r.fecha_respuesta)
				.ToListAsync(cancellationToken);

			var grouped = list
				.GroupBy(r => r.submission_id)
				.Select(g => new RespuestaSummaryDto
			{
				id = g.Key.HasValue ? $"sub-{g.Key.Value}" : $"resp-{g.Max(x => x.id_respuesta)}",
				encuestaId = idEncuesta,
				creadaEn = new DateTimeOffset(g.Max(x => x.fecha_respuesta))
			})
			.OrderByDescending(x => x.creadaEn)
			.ToList();

			return grouped;
		}
	}
}
