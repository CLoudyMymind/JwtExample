using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Common;

public class AuthOptions
{
    public const string Issuer = "JwtProj";  // Закинуть в json
    public const string Audience = "Jwt";
    const string Key = "JwtExampleKey";   // вынести в хранилище ключей
    public static readonly DateTime LifetimeToken = DateTime.Now.AddMinutes(15);
    public static readonly DateTime LifeTimeRefreshToken = DateTime.Now.AddDays(7);
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}