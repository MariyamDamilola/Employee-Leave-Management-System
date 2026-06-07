using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;

namespace EmployeeLeaveManagementSystem.Repositories.Interfaces;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequest>> GetAllleaveRequests();
    
    Task<LeaveRequest> GetleaveRequestsbyId(int leaveRequestId);
    
    Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int employeeId);

    
    Task<LeaveRequest> SubmitLeaveRequest(CreateLeaveDTO createLeaveDto);
    
    Task<LeaveRequest> UpdateLeaveRequest(int leaveRequestId, UpdateLeaveDTO updateLeaveDto);
    
    Task<bool> DeleteLeaveRequest(int leaveRequestId);

    Task<LeaveRequest> AcceptLeaveRequest(int leaveRequestId);

    Task<LeaveRequest> RejectLeaveRequest(int leaveRequestId);

    Task<IEnumerable<LeaveRequest>> FilterLeaveRequestsByStatus(string status);

    Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave();

    Task<Dictionary<string, int>> GetLeaveStatisticsByDepartment();

}