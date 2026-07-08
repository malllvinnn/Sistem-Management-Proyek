using FluentValidation;
using SistemManagementProjectAPI.DTOs.TaskItem;

namespace SistemManagementProjectAPI.Validators;

public class CreateTaskItemValidator : AbstractValidator<CreateTaskItemDto>
{
    public CreateTaskItemValidator()
    {
        RuleFor((x) => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
        
        RuleFor((x) => x.ProjectId)
            .NotEqual(Guid.Empty).WithMessage("Project Id is required");
    }
}