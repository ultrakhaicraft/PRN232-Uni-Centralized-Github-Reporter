using GithubReporterRepository.EntityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubReporterService.Utilities;

public class TokenProvider
{
	private readonly IConfiguration _configuration;

	public TokenProvider(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string generateAccessToken(Account account)
	{

		var jwtSetting = _configuration.GetSection("JwtSetting");
		var secretKey = jwtSetting["SecretKey"];
		var issuer = jwtSetting["Issuer"];
		var audience = jwtSetting["Audience"];
		var expiryMinutes = int.Parse(jwtSetting["AccessTokenExpiration"] ?? "60");
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//Add claims
		var claims = new[]
		{
			new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, account.Username),
			new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, account.Email),
			new System.Security.Claims.Claim("AccountId", account.Id.ToString())
		};

		//Generate token
		var token = new JwtSecurityToken(
			issuer: issuer,
			audience: audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
			signingCredentials: credentials
		);



		return new JwtSecurityTokenHandler().WriteToken(token);	
	}
}
