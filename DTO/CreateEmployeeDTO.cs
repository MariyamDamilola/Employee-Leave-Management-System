namespace EmployeeLeaveManagementSystem.DTO;

public class CreateEmployeeDTO
{
    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public string Department { get; set; }
    
    public DateTime DateJoined { get; set; }
}