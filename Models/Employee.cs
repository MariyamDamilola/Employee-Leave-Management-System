namespace EmployeeLeaveManagementSystem.Models;

public class Employee
{
    public int Id { get; set; }
    
    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public string Department { get; set; }
    
    public string CompanyBadgeId { get; set; }
    
    public DateTime DateJoined { get; set; }

    
    //One employee can have many requests
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();


}