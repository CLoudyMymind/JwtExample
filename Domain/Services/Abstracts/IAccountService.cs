using Domain.Models;
using Domain.Monads;

namespace Domain.Services.Abstracts;

public interface IAccountService
{
    Task<Result<bool, Exception>> Register(User user);
    Task<Result<string, Exception>> StartVerifyAccount(string email);
}