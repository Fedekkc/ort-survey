using System.ComponentModel.DataAnnotations;

namespace OrtSurvey.Dtos;

public class AuthLoginRequest
{
    [Required(ErrorMessage = "El campo Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo Email debe ser una direccion valida.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo Password es obligatorio.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "El campo Password debe tener entre {2} y {1} caracteres.")]
    public string Password { get; set; } = string.Empty;
}
