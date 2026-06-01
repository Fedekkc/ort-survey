namespace OrtSurvey.Dtos;

public class AuthUserDto
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Nombre { get; init; } = string.Empty;
    public string Genero { get; init; }
}
