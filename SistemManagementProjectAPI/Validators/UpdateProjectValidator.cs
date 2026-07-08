using FluentValidation;
using SistemManagementProjectAPI.DTOs.Project;

namespace SistemManagementProjectAPI.Validators;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectValidator()
    {
        RuleFor((x) => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
        
        RuleFor((x) => x.Status)
            .IsInEnum().WithMessage("Status invalid");
    }
}