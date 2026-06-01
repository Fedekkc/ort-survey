using OrtSurvey.Dtos;

namespace OrtSurvey.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(AuthRegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(AuthLoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthUserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default);
}
