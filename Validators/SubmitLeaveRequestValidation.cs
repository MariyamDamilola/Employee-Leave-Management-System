using EmployeeLeaveManagementSystem.DTO;
using FluentValidation;

namespace EmployeeLeaveManagementSystem.Validators;

public class SubmitLeaveRequestValidation : AbstractValidator<SubmitLeaveRequestDto>
{
    public SubmitLeaveRequestValidation()
    {
        RuleFor(x=>x.EmployeeId).GreaterThan(0)
            .WithMessage("EmployeeId must be greater than 0");

        RuleFor(x => x.LeaveType).NotEmpty()
            .WithMessage("Leave Type is required")
            .MaximumLength(100)
            .WithMessage("Leave Type cannot exceed 100 characters");
        
        RuleFor(x=>x.StartDate).NotEmpty()
            .WithMessage("Start Date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start Date cannot be in the past");
        
        RuleFor(x=>x.EndDate).NotEmpty()
            .WithMessage("End Date is required")
            .GreaterThan(x=>x.StartDate)
            .WithMessage("End Date must be after Start Date");
        
        RuleFor(x => x.Reason).NotEmpty()
            .WithMessage("Reason is required")
            .MaximumLength(500)
            .WithMessage("Reason cannot  exceed 500 characters");
    }
}