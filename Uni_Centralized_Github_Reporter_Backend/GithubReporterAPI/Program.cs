using GithubReporterRepository;
using GithubReporterRepository.Core;
using GithubReporterRepository.Interface;
using GithubReporterRepository.Models;
using GithubReporterService.Core;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var JWTsettings = builder.Configuration.GetSection("JwtSetting");
    var key = System.Text.Encoding.UTF8.GetBytes(JWTsettings["SecretKey"]!);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = JWTsettings["Issuer"],
        ValidAudience = JWTsettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
	};
});

builder.Services.AddDbContext<GithubReporterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
   

//Add Services from other layers (TODO: Add a wrapper class to handle this)
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
