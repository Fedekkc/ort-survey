namespace OrtSurvey.Helpers;

public static class EncuestaCodigoHelper
{
    public static string Generar()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
