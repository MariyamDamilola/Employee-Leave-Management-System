using EmployeeLeaveManagementSystem.DTO;
using FluentValidation;

namespace EmployeeLeaveManagementSystem.Validators;

public class LeaveActionRequestValidation : AbstractValidator<LeaveActionRequestDto>
{
    public LeaveActionRequestValidation()
    {
        RuleFor(x=>x.ApproverId)
            .GreaterThan(0)
            .WithMessage("ApproverId must be greater than 0");
        
        
        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .WithMessage("Reason cannot  exceed 500 characters");
    }
}