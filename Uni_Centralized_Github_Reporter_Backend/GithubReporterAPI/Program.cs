using AutoMapper;
using GithubReporterAPI.Utilities;
using GithubReporterRepository;
using GithubReporterRepository.Core;
using GithubReporterRepository.Interface;
using GithubReporterRepository.Models;
using GithubReporterService.Core;
using GithubReporterService.Interface;
using GithubReporterService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowFrontend", policy =>
	{
		policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials();
	});
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Lab 2 API", Version = "v1" });

	// 1. Define the 'Bearer' security scheme
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter your JWT token in the text input below.\r\n\r\nExample: 'eyJhbGciOiJIUzI1Ni...' (Do not prefix with 'Bearer ')"
	});

	// 2. Make sure Swagger uses that scheme globally
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

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

builder.Services.AddScoped<DbContext>(provider =>
	provider.GetRequiredService<GithubReporterContext>());


//Add Services from other layers (TODO: Add a wrapper class to handle this)
builder.Services.AddInfrastructure();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
