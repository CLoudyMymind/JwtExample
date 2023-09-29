using Domain.Models;
using Domain.Monads;

namespace Domain.Services.Abstracts;

public interface ITokenService
{
     Task<Result<string, Exception>> GetTokenAsync(string login, string password);
     Task<Result<Token, Exception>> RefreshAsync(Token token);
     Task<Result<bool, Exception>> RevokeAsync(string token);
}