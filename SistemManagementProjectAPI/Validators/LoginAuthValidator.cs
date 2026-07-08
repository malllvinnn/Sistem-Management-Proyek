using FluentValidation;
using SistemManagementProjectAPI.DTOs.Auth;

namespace SistemManagementProjectAPI.Validators;

public class LoginAuthValidator : AbstractValidator<LoginDto>
{
    public LoginAuthValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}