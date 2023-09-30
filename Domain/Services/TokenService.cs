using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Domain.Common;
using Domain.Context;
using Domain.Models;
using Domain.Monads;
using Domain.Ports;
using Domain.Services.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services;

public class TokenService : ITokenService
{
    private readonly JwtContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public TokenService(JwtContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<string, Exception>> GetTokenAsync(string login, string password)
    {
        try
        {
            var user = await _db.Users.FirstOrDefaultAsync(c => c.Login.ToLower() == login.ToLower());
            if (user is null)
                return new Result<string, Exception>(new Exception("Такого пользователя нету"));
            if (!_passwordHasher.Verify(password, user.Password))
                return new Result<string, Exception>(new Exception("Пароль не вверный"));

            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRefreshToken(),
                RefreshTokenExpiryTime = AuthOptions.LifeTimeRefreshToken
            };
            await _db.RefreshTokens.AddAsync(newRefreshToken);
            await _db.SaveChangesAsync();
            return GenerateAccessToken(user, await GetRolesName(user));
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<Token, Exception>> RefreshAsync(Token token)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var user = await _db.Users.FirstOrDefaultAsync(c =>
                principal.Identity != null && c.Login == principal.Identity.Name);
            if (user is null)
                return new Result<Token, Exception>(new Exception("Такого пользователя нету"));
            var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token.RefreshToken);
            if (refreshToken is null)
                return new Result<Token, Exception>(new Exception("Ошибка такого токена нету"));
            if (refreshToken.RefreshTokenExpiryTime <= DateTimeOffset.Now)
                await RevokeAsync(refreshToken.Token);
            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateRefreshToken(),
                RefreshTokenExpiryTime = AuthOptions.LifeTimeRefreshToken
            };
            await _db.RefreshTokens.AddAsync(newRefreshToken);
            await _db.SaveChangesAsync();
            var newToken = new Token
            {
                AccessToken = GenerateAccessToken(user, await GetRolesName(user)),
                RefreshToken = newRefreshToken.Token
            };
            return newToken;
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public async Task<Result<bool, Exception>> RevokeAsync(string token)
    {
        var findToken = await _db.RefreshTokens.FirstOrDefaultAsync(c => c.Token == token);
        if (findToken is null)
            return new Result<bool, Exception>(new Exception("Ошибка такого токена нету"));
        _db.RefreshTokens.Remove(findToken);
        await _db.SaveChangesAsync();
        return true;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[128];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateAccessToken(User user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ToString()),
            new(JwtRegisteredClaimNames.Name, user.Surname!),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var signinCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            null,
            expires: AuthOptions.LifetimeToken,
             signinCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }

    private async Task<List<string>> GetRolesName(User user)
    {
        return await _db.UserRoles.Include(r => r.Role)
            .Where(r => user.Id == r.UserId).Where(u => u.RoleId == u.Role.Id).Select(c => c.Role.Name)
            .ToListAsync();
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }
}