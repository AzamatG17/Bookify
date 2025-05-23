﻿using Bookify.Application.Configurations;
using Bookify.Application.Interfaces;
using Bookify.Domain_.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookify.Application.Services;

internal sealed class JwtTokenHandler : IJwtTokenHandler
{
    private readonly JwtOptions _options;

    public JwtTokenHandler(IOptions<JwtOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public string GenerateToken(User user, IEnumerable<string> roles)
    {
        var claims = GetClaims(user, roles);
        var signingKey = GetClaimingKey();
        var securityToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            signingCredentials: signingKey,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresInHours));

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    private static List<Claim> GetClaims(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials GetClaimingKey()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return signingKey;
    }
}
