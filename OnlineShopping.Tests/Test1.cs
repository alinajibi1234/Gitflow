using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectGitFlow.Controllers;
using ProjectGitFlow.Models.Dtos;
using ProjectGitFlow.Services;

namespace ProjectGitFlow.Tests;

[TestClass]
public class UsersControllerTests
{
    private Mock<IUserService> _mockUserService = null!;
    private UsersController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UsersController(_mockUserService.Object);
    }

    #region Register Tests

    [TestMethod] // Test 1: Valid Registration -> Returns 200 OK
    public void Register_ValidData_Returns200Ok()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "alice@example.com",
            Password = "Password123!"
        };

        _mockUserService.Setup(s => s.Register(request))
                        .Returns(AuthResultDto.Ok("User registered successfully"));

        // Act
        var result = _controller.Register(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod] // Test 2: Email Already Exists / Fails -> Returns 409 Conflict
    public void Register_EmailAlreadyExists_Returns409Conflict()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "alice@example.com",
            Password = "Password123!"
        };

        _mockUserService.Setup(s => s.Register(request))
                        .Returns(AuthResultDto.Fail("Email already exists"));

        // Act
        var result = _controller.Register(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
        var conflictResult = (ConflictObjectResult)result;
        Assert.AreEqual(409, conflictResult.StatusCode);
    }

    [TestMethod] // Test 3: Invalid ModelState -> Returns 400 BadRequest
    public void Register_InvalidModelState_Returns400BadRequest()
    {
        // Arrange
        var request = new RegisterRequestDto { Email = "not-an-email", Password = "" };
        _controller.ModelState.AddModelError("Email", "The Email field is not a valid e-mail address.");

        // Act
        var result = _controller.Register(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.AreEqual(400, badRequestResult.StatusCode);
        // Verificer at servicen ikke bliver kaldt unødigt ved valideringsfejl
        _mockUserService.Verify(s => s.Register(It.IsAny<RegisterRequestDto>()), Times.Never);
    }

    #endregion

    #region Login Tests

    [TestMethod] // Test 4: Valid Credentials -> Returns 200 OK
    public void Login_ValidCredentials_Returns200Ok()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "bob@example.com",
            Password = "Password123!"
        };

        _mockUserService.Setup(s => s.Login(request))
                        .Returns(AuthResultDto.Ok("Login successful"));

        // Act
        var result = _controller.Login(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod] // Test 5: Wrong Password / User Not Found -> Returns 401 Unauthorized
    public void Login_InvalidCredentials_Returns401Unauthorized()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            Email = "bob@example.com",
            Password = "WrongPassword"
        };

        _mockUserService.Setup(s => s.Login(request))
                        .Returns(AuthResultDto.Fail("Invalid credentials"));

        // Act
        var result = _controller.Login(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        var unauthorizedResult = (UnauthorizedObjectResult)result;
        Assert.AreEqual(401, unauthorizedResult.StatusCode);
    }

    [TestMethod] // Test 6: Invalid ModelState -> Returns 400 BadRequest
    public void Login_InvalidModelState_Returns400BadRequest()
    {
        // Arrange
        var request = new LoginRequestDto { Email = "", Password = "" };
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = _controller.Login(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.AreEqual(400, badRequestResult.StatusCode);
        _mockUserService.Verify(s => s.Login(It.IsAny<LoginRequestDto>()), Times.Never);
    }

    #endregion
}