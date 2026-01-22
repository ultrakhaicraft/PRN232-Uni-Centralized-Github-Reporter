using GithubReporterRepository;
using GithubReporterRepository.Core;
using GithubReporterRepository.Interface;
using GithubReporterService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

//Add Services from other layers (TODO: Add a wrapper class to handle this)
//builder.Services.AddScoped<GithubReporterRepository.Interface.IUnitOfWork, GithubReporterRepository.Core.UnitOfWork>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();   
builder.Services.AddSingleton<InMemoryUserStoreTemp>();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddScoped<GithubReporterService.Interface.IAuthenticationService, GithubReporterService.Core.AuthenticationService>();

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
