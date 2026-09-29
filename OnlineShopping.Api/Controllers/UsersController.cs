using Microsoft.AspNetCore.Mvc;
using ProjectGitFlow.Models.Dtos;
using ProjectGitFlow.Services;


namespace ProjectGitFlow.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = _userService.Register(request);

        if (!result.Success) return Conflict(new { message = result.Message });
        return Ok(new { message = result.Message });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = _userService.Login(request);

        if (!result.Success) return Unauthorized(new { message = result.Message });
        return Ok(new { message = result.Message });
    }
}