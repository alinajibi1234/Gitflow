namespace ProjectGitFlow.Models.Dtos;

public class AuthResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static AuthResultDto Ok(string message) => new() { Success = true, Message = message };
    public static AuthResultDto Fail(string message) => new() { Success = false, Message = message };
}