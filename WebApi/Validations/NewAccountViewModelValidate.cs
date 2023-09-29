using FluentValidation;
using JwtExample.ViewModels;

namespace WebApi.Validations;

public class NewAccountViewModelValidate : AbstractValidator<NewAccountViewModel>
{
    public NewAccountViewModelValidate()
    {
        RuleFor(c => c.Password)
            .Matches("(?=^.{8,15}$)(?=.*\\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\\s).*$")
            .WithMessage(NewAccountError.PasswordInCorrect)
            .NotNull().WithMessage(NewAccountError.PasswordNullable);
        RuleFor(c => c.Email).Matches(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$")
            .WithMessage(NewAccountError.EmailInCorrect);
    }
}