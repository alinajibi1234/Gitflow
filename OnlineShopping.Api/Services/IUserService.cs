using ProjectGitFlow.Models.Dtos;

namespace ProjectGitFlow.Services;

public interface IUserService
{
    AuthResultDto Register(RegisterRequestDto request);
    AuthResultDto Login(LoginRequestDto request);
}