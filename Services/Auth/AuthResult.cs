using OrtSurvey.Dtos;

namespace OrtSurvey.Services;

public enum AuthErrorCode
{
    None,
    InvalidRequest,
    EmailInUse,
    InvalidCredentials,
    UserNotFound
}

public class AuthResult
{
    public bool Succeeded { get; init; }
    public AuthErrorCode ErrorCode { get; init; } = AuthErrorCode.None;
    public string ErrorMessage { get; init; }
    public AuthUserDto User { get; init; }

    public static AuthResult Success(AuthUserDto user)
    {
        return new AuthResult { Succeeded = true, User = user };
    }

    public static AuthResult Fail(AuthErrorCode code, string message)
    {
        return new AuthResult { Succeeded = false, ErrorCode = code, ErrorMessage = message };
    }
}
