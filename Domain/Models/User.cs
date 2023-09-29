namespace Domain.Models;

public sealed class User : BaseEntity
{
    public  string Login { get; set; }
    public  string Password { get; set; }
    public  string Surname { get; set; }
    public  string Email { get; set; }
    public  bool UserConfirmed { get; set; }
    public User(string login, string password, string surname, string email)
    {
        Login = login;
        Password = password;
        Surname = surname;
        Email = email;
        Id = Guid.NewGuid();
        CreateDateTimeOffset = DateTimeOffset.Now;
        UserConfirmed = false;
    }

    public User()
    {
       
    }
}