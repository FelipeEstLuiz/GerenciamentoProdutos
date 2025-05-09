using Application.Domain.Entities;
using Application.Domain.Interfaces.Services;
using Application.Domain.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Core.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public Task<string> GerarToken(Usuario usuario)
    {
        DateTime _expiresAt = DateTime.UtcNow.AddHours(1);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Name, usuario.Email),
                new(ClaimTypes.NameIdentifier, usuario.Nome)
            ]),
            Expires = _expiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(ClsGlobal.GetTokenKey(configuration)),
                SecurityAlgorithms.HmacSha256Signature
            ),
            IssuedAt = _expiresAt
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult<string>(tokenHandler.WriteToken(token));
    }
}
