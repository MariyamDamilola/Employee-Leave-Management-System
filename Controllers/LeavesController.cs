using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
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
    
    
    [HttpGet("GetEmployeeLeaveHistory/{employeeId}")]
    public async Task<IActionResult> GetEmployeeLeaveHistory(int employeeId)
    {
        var leaveRequestHistory = await _leaveRepository.GetEmployeeLeaveHistory(employeeId);
        return Ok(leaveRequestHistory);
    }
    

    [HttpPost("SubmitLeaveRequest")]
    public async Task<IActionResult> SubmitLeaveRequest(CreateLeaveDTO createLeaveDto)
    {
        var submitLeaveRequest = await _leaveRepository.SubmitLeaveRequest(createLeaveDto);
        return Ok(submitLeaveRequest);
    }


    [HttpPut("UpdateEmployeeLeaveHistory/{employeeId}")]
    public async Task<IActionResult> UpdateLeaveRequest(int leaveRequestId, CreateLeaveDTO createLeaveDto)
    {
        var updatedLeaveRequest = await _leaveRepository.UpdateLeaveRequest(leaveRequestId, createLeaveDto);
        return Ok(updatedLeaveRequest);
    }

    [HttpDelete("DeleteLeaveRequest/{id}")]
    public async Task<IActionResult> DeleteLeaveRequest(int id)
    {
        var deletedLeaveRequest = await _leaveRepository.DeleteLeaveRequest(id);
        return Ok(deletedLeaveRequest);
    }

    [HttpPut("RejectLeaveRequest/{id}")]
    public async Task<IActionResult> RejectLeaveRequest(int id)
    {
        var rejectedRequest = await _leaveRepository.RejectLeaveRequest(id);
        return Ok(rejectedRequest);
    }

    [HttpGet("FilterByStatus")]
    public async Task<IActionResult> FilterLeaveRequestsByStatus(string status)
    {
        var requests = await _leaveRepository.FilterLeaveRequestsByStatus(status);
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