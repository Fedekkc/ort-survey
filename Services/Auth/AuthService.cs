using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrtSurvey.Context;
using OrtSurvey.Dtos;
using OrtSurvey.Models;

namespace OrtSurvey.Services.Auth;

public class AuthService : IAuthService
{
    private readonly OrtSurveyDataBase _db;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AuthService(OrtSurveyDataBase db, IPasswordHasher<Usuario> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> RegisterAsync(AuthRegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidRequest, "Solicitud invalida.");
        }

        var email = (request.Email ?? string.Empty).Trim();
        var nombre = (request.Nombre ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(request.Password))
        {
            return AuthResult.Fail(AuthErrorCode.InvalidRequest, "Datos de registro invalidos.");
        }

        var emailExists = await _db.Usuarios.AnyAsync(u => u.correo == email, cancellationToken);
        if (emailExists)
        {
            return AuthResult.Fail(AuthErrorCode.EmailInUse, "El email ya esta registrado.");
        }

        var user = new Usuario
        {
            correo = email,
            nombre = nombre,
            genero = request.Genero,
            password_hash = string.Empty
        };

        user.password_hash = _passwordHasher.HashPassword(user, request.Password);

        _db.Usuarios.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return AuthResult.Success(MapUser(user));
    }

    public async Task<AuthResult> LoginAsync(AuthLoginRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidRequest, "Solicitud invalida.");
        }

        var email = (request.Email ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials, "Credenciales invalidas.");
        }

        var user = await _db.Usuarios.SingleOrDefaultAsync(u => u.correo == email, cancellationToken);
        if (user == null)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials, "Credenciales invalidas.");
        }

        var verification = _passwordHasher.VerifyHashedPassword(user, user.password_hash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
        {
            return AuthResult.Fail(AuthErrorCode.InvalidCredentials, "Credenciales invalidas.");
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.password_hash = _passwordHasher.HashPassword(user, request.Password);
            await _db.SaveChangesAsync(cancellationToken);
        }

        return AuthResult.Success(MapUser(user));
    }

    public async Task<AuthUserDto> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var user = await _db.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.id_usuario == userId, cancellationToken);

        return user == null ? null : MapUser(user);
    }

    private static AuthUserDto MapUser(Usuario user)
    {
        return new AuthUserDto
        {
            Id = user.id_usuario,
            Email = user.correo,
            Nombre = user.nombre,
            Genero = user.genero
        };
    }
}
