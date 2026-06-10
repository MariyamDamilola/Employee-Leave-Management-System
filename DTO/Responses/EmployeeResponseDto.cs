namespace EmployeeLeaveManagementSystem.DTO.Responses;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    
    public string FullName { get; set; }
    
    public string Department { get; set; }
    
    public string Email { get; set; }
    
    public DateTime DateJoined { get; set; }
}