using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;

namespace EmployeeLeaveManagementSystem.Repositories.Interfaces;

public interface ILeaveRepository
{
    Task<IEnumerable<LeaveRequest>> GetAllleaveRequests();
    
    Task<LeaveRequest> GetleaveRequestsbyId(int leaveRequestId);
    
    Task<LeaveRequest> SubmitLeaveRequest(SubmitLeaveRequestDto submitLeaveRequestDto);
    
    Task<LeaveRequest> UpdateLeaveRequest(int leaveRequestId, SubmitLeaveRequestDto submitLeaveRequestDto);
    
    Task<bool> DeleteLeaveRequest(int leaveRequestId);
    
    Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int employeeId);
    
    Task<LeaveRequest> ApproveLeaveRequest(int leaveRequestId, LeaveActionRequestDto  leaveActionRequestDto);
    
    Task<LeaveRequest> RejectLeaveRequest(int leaveRequestId, LeaveActionRequestDto leaveActionRequestDto);

    Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatus(string status);
    
    Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave();
    
    Task<Dictionary<string, int>> GetLeaveStatisticsByDepartment();



    

   



}