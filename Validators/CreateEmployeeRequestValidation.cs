using EmployeeLeaveManagementSystem.DTO;
using FluentValidation;

namespace EmployeeLeaveManagementSystem.Validators;

public class CreateEmployeeRequestValidation : AbstractValidator<CreateEmployeeRequestDto>
{
    public  CreateEmployeeRequestValidation()
    {
        RuleFor(x => x.FullName).NotEmpty().Length(3, 100)
            .WithMessage("FullName is required and must be between 3 and 100 characters");
        
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress()
            .WithMessage("The email format is invalid");
        
        RuleFor(x => x.Department).NotEmpty().WithMessage("Department is required");
    }
}