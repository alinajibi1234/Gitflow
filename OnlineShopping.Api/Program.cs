using Microsoft.AspNetCore.Identity;
using ProjectGitFlow.Models;
using ProjectGitFlow.Repositories;
using ProjectGitFlow.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }