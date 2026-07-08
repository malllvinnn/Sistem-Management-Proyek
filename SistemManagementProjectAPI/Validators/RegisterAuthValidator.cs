using FluentValidation;
using SistemManagementProjectAPI.DTOs.Auth;

namespace SistemManagementProjectAPI.Validators;

public class RegisterAuthValidator : AbstractValidator<RegisterDto>
{
    public RegisterAuthValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("The password must contain uppercase letters, lowercase letters, and numbers.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.Password).WithMessage("Password and confirm password do not match");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MinimumLength(2).WithMessage("Last name must be between 2 and 50 characters")
            .MaximumLength(50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage("First name must be 50 characters")
            .When(x => !string.IsNullOrEmpty(x.FirstName));
    }
}