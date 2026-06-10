namespace EmployeeLeaveManagementSystem.DTO.Responses;

public class LeaveApprovalResponseDto
{
    public int LeaveRequestId { get; set; }
    
    public int ApproverId { get; set; }
    
    public string Action { get; set; }
    
    public string Reason { get; set; }
    
    public DateTime DateActed { get; set; }
}