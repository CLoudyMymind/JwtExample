using Domain.Models;

namespace JwtExample.ViewModels;

public class NewAccountViewModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string Surname { get; set; }

    public static User MapToDomain(NewAccountViewModel model)
    {
        return new User
        {
            Login = model.Login,
            Password = model.Password,
            Surname = model.Surname,
            Email = model.Email,
            Id = default,
            CreateDateTimeOffset = default,
            UserConfirmed = false
        };
    }
}