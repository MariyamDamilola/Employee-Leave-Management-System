using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LeavesController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;
    
    public  LeavesController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    [HttpGet("GetAllLeaveRequests")]
    public async Task<IActionResult> GetAllLeaveRequests()
    {
        var leaves = await _leaveRepository.GetAllleaveRequests();
        return Ok(leaves);
    }
    

    [HttpGet("GetLeavesRequestById/{id}")]
    public async Task<IActionResult> GetLeaveRequestById(int id)
    {
        var leaveRequests = await _leaveRepository.GetleaveRequestsbyId(id);
        return Ok(leaveRequests);
    }
    
    
    [HttpPost("SubmitLeaveRequest")]
    public async Task<IActionResult> SubmitLeaveRequest(SubmitLeaveRequestDto submitLeaveRequestDto)
    {
        var submitLeaveRequest = await _leaveRepository.SubmitLeaveRequest(submitLeaveRequestDto);
        return Ok(submitLeaveRequest);
    }

    
    [HttpPut("UpdateEmployeeLeaveHistory/{Id}")]
    public async Task<IActionResult> UpdateLeaveRequest(int id, SubmitLeaveRequestDto submitLeaveRequestDto)
    {
        var updatedLeaveRequest = await _leaveRepository.UpdateLeaveRequest(id, submitLeaveRequestDto);
        return Ok(updatedLeaveRequest);
    }
    
    [HttpDelete("DeleteLeaveRequest/{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var deletedLeaveRequest = await _leaveRepository.DeleteLeaveRequest(id);
            return Ok(deletedLeaveRequest);
        }
    
    
    [HttpGet("GetEmployeeLeaveHistory/{employeeId}")]
    public async Task<IActionResult> GetEmployeeLeaveHistory(int employeeId)
    {
        var leaveRequestHistory = await _leaveRepository.GetEmployeeLeaveHistory(employeeId);
        return Ok(leaveRequestHistory);
    }
    
    
    [HttpPut("ApproveLeaveRequest/{id}")]
    public async Task<IActionResult> ApproveLeaveRequest(int id, LeaveActionRequestDto leaveActionRequestDto)
    {
        var acceptedRequest = await _leaveRepository.ApproveLeaveRequest(id, leaveActionRequestDto);
        return Ok(acceptedRequest);
    }
    
    
    [HttpPut("RejectLeaveRequest/{id}")]
    public async Task<IActionResult> RejectLeaveRequest(int id, LeaveActionRequestDto leaveActionRequestDto)
    {
        var rejectedRequest = await _leaveRepository.RejectLeaveRequest(id, leaveActionRequestDto);
        return Ok(rejectedRequest);
    }

    
    [HttpGet("GetByStatus")]
    public async Task<IActionResult> GetLeaveRequestsByStatus(string status)
    {
        var requests = await _leaveRepository.GetLeaveRequestsByStatus(status);
        return Ok(requests);
    }

    [HttpGet("CurrentlyOnLeave")]
    public async Task<IActionResult> GetEmployeesCurrentlyOnLeave()
    {
        var employees = await _leaveRepository.GetEmployeesCurrentlyOnLeave();
        return Ok(employees);
    }

    [HttpGet("StatisticsByDepartment")]
    public async Task<IActionResult> GetLeaveStatisticsByDepartment()
    {
        var stats = await _leaveRepository.GetLeaveStatisticsByDepartment();
        return Ok(stats);
    }

}