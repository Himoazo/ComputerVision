using ComputerVision.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComputerVision.Data.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}


public class TokenService : ITokenService
{
    private readonly IConfiguration _config;  //hämtar Issuer och Audience från JWT appsettings.json

    private readonly SymmetricSecurityKey _key; // hämtar nyckeln från JWT appsettings.json
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
    }
    public string CreateToken(AppUser user)
    {
        // Ger användarens info till claims
        var Claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!)
        };

        // Kryptering av token
        var encrypt = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        //Skapa token komponenter
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(Claims),
            Expires = DateTime.Now.AddDays(3),
            SigningCredentials = encrypt,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor); // Skapar JWT token

        return tokenHandler.WriteToken(token);
    }
}
