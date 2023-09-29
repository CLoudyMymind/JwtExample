using Domain.Context;
using Domain.Enums;
using Domain.Models;
using Domain.Monads;
using Domain.Ports;
using Domain.Services.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services;

public class AccountService : IAccountService
{
    private readonly JwtContext _db;
    private readonly IPasswordHasher _passwordHasher;
    

    public AccountService(JwtContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<bool, Exception>> Register(User user)
    {
        var newUser = new User
        {
            Login = user.Login,
            Password = _passwordHasher.Hash(user.Password),
            Surname = user.Surname,
            Email = user.Email,
            Id = Guid.NewGuid(),
            CreateDateTimeOffset = DateTimeOffset.Now,
            UserConfirmed = false
        };
        var userRole = new UserRole
        {
            Id = 0,
            User = newUser,
            UserId = newUser.Id,
            Role = null,
            RoleId = (int)PermissionType.User
        };
        await _db.UserRoles.AddAsync(userRole);
        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Result<string, Exception>> StartVerifyAccount(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
            return new Result<string, Exception>(new Exception("такого пользователя нету"));
        var newCodes = new ConfirmCode
        {
            User = user,
            UserId = user.Id,
            Codes = Random.Shared.Next(1000, 9999).ToString(),
            ExpiredDateTime = DateTimeOffset.Now.AddMinutes(15)
        };
        await _db.ConfirmCodes.AddAsync(newCodes);
        await _db.SaveChangesAsync();
        return newCodes.Codes;
    }
}