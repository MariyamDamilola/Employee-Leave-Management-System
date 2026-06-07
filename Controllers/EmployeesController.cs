using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementSystem.Controllers;
[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public  EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet("GetAllEmployees")]
    
    public async Task<IActionResult> GetAllEmployee()
    {
        var employees = await _employeeRepository.GetAllEmployees();
        return Ok(employees);
    }

    [HttpGet("GetEmployeeById/{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _employeeRepository.GetEmployeeById(id);
        return Ok(employee);
    }

    [HttpPost("CreateEmployee")]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeDTO createEmployeeDto)
    {
        var createdEmployee = await _employeeRepository.CreateEmployee(createEmployeeDto);
        return Ok(createdEmployee);
    }

    [HttpPut("UpdateEmployee/{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDTO updateEmployeeDto)
    {
        var UpdatedEmployee = await _employeeRepository.UpdateEmployee(id, updateEmployeeDto);
        return Ok(UpdatedEmployee);
    }

    [HttpDelete("DeleteEmployee/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var isDeleted = await _employeeRepository.DeleteEmployee(id);
        return Ok(isDeleted);
    }
    
    
    
    
}