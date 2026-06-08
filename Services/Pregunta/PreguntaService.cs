using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrtSurvey.Context;
using OrtSurvey.Dtos.Pregunta;
using OrtSurvey.Models;


namespace OrtSurvey.Services.Pregunta;

public class PreguntaService : IPreguntaService
{
    private readonly OrtSurveyDataBase _db;

    public PreguntaService(OrtSurveyDataBase db)
    {
        _db = db;
    }

    //get pregunta por ID
    public async Task<PreguntaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pregunta = await _db.Preguntas
            .AsNoTracking()
            .Include(p => p.Opciones)
            .Include(p => p.Respuestas)
            .FirstOrDefaultAsync(p => p.id_pregunta == id, cancellationToken);

        return pregunta == null ? null : MapPregunta(pregunta);
    }


    //get preguntas por encuesta
    public async Task<List<PreguntaDto>> GetByEncuestaAsync(int idEncuesta, CancellationToken cancellationToken = default)
    {
        var preguntas = await _db.Preguntas
            .AsNoTracking()
            .Include(p => p.Opciones)
            .Where(p => p.id_encuesta == idEncuesta)
            .OrderBy(p => p.id_pregunta)
            .ToListAsync(cancellationToken);

        return preguntas.Select(MapPregunta).ToList();
    }


    //crear pregunta, hay que testear 
    public async Task<PreguntaDto> CreateAsync(CreatePreguntaRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));

        // tengo que ir por la direccion del modelo , no me deja de otra manera
        var pregunta = new OrtSurvey.Models.Pregunta
        {
            texto = request.Texto,
        };

        if (request.Opciones != null && request.Opciones.Any())
        {
            foreach (var opcion in request.Opciones)
            {
                // no se que hice aca
                pregunta.Opciones.Add(new Opcion { texto = opcion.texto });
            }
        }

        _db.Preguntas.Add(pregunta);
        await _db.SaveChangesAsync(cancellationToken);

        return MapPregunta(pregunta);
    }


    //mapeo 
    //no me deja usar Pregunta, con OrtSurvey.Models.Pregunta se soluciona. ENCIMA CON Opcion me deja, odio todo.
    private static PreguntaDto MapPregunta(OrtSurvey.Models.Pregunta p)
    {
        return new PreguntaDto
        {
            Id = p.id_pregunta,
            Texto = p.texto,
            IdEncuesta = p.id_encuesta
        };
    }
}