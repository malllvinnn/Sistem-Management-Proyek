using FluentValidation;
using SistemManagementProjectAPI.DTOs.Developer;

namespace SistemManagementProjectAPI.Validators;

public class UpdateDeveloperValidator : AbstractValidator<UpdateDeveloperDto>
{
    public UpdateDeveloperValidator()
    {
        RuleFor((x) => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(150).WithMessage("Name must not exceed 150 characters");
        
        RuleFor((x) => x.Skills)
            .NotEmpty().WithMessage("Skills is required")
            .MaximumLength(100).WithMessage("Skills must not exceed 100 characters");
    }
}