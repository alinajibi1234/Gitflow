using Microsoft.AspNetCore.Identity;
using ProjectGitFlow.Models;
using ProjectGitFlow.Models.Dtos;
using ProjectGitFlow.Repositories;

namespace ProjectGitFlow.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public AuthResultDto Register(RegisterRequestDto request)
    {
        if (_userRepository.EmailExists(request.Email))
        {
            return AuthResultDto.Fail("Email is already registered.");
        }

        var user = new User { Email = request.Email };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _userRepository.Add(user);

        return AuthResultDto.Ok("User registered successfully.");
    }

    public AuthResultDto Login(LoginRequestDto request)
    {
        var user = _userRepository.GetByEmail(request.Email);
        if (user is null)
        {
            // Bevidst vagt: afslør ikke om det er emailen eller passwordet der er forkert.
            return AuthResultDto.Fail("Invalid email or password.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user, user.PasswordHash, request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return AuthResultDto.Fail("Invalid email or password.");
        }

        return AuthResultDto.Ok("Login successful.");
    }
}