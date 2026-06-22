using OrtSurvey.Dtos.Pregunta;

namespace OrtSurvey.Services.Pregunta;

public interface IPreguntaService
{
    Task<PreguntaDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<PreguntaDto>> GetByEncuestaAsync(int idEncuesta, CancellationToken cancellationToken = default);
    Task<PreguntaDto> CreateAsync(CreatePreguntaRequest request, CancellationToken cancellationToken = default);

    //no hace falta el update, no tiene sentido actualizar o borrar una pregunta. lo dejo porsiaca.
    //Task<bool> UpdateAsync(UpdatePreguntaRequest request, CancellationToken cancellationToken = default);
    //Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}