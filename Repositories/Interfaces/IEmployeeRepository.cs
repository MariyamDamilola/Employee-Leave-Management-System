using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;

namespace EmployeeLeaveManagementSystem.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployees();
    
    Task<Employee?> GetEmployeeById(int id);
    
    Task<Employee> CreateEmployee(CreateEmployeeRequestDto createEmployeeRequestDto);
    
    Task<Employee> UpdateEmployee(int id, UpdateEmployeeRequestDto updateEmployeeRequestDto );
    
    Task<bool> DeleteEmployee(int id);

}