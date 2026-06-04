using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;

namespace EmployeeLeaveManagementSystem.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployees();
    
    Task<Employee> GetEmployeeById(int id);
    
    Task<Employee> CreateEmployee(CreateEmployeeDTO createEmployeeDto);
    
    Task<Employee> UpdateEmployee(int id, CreateEmployeeDTO createEmployeeDto);
    
    Task<bool> DeleteEmployee(int id);

}