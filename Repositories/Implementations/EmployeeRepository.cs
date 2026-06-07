using EmployeeLeaveManagementSystem.Data;
using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSystem.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EmployeeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        
    }   
    
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        var employees = await _dbContext.Employees.ToListAsync();
        return employees;
    }

    public async Task<Employee> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x=> x.Id == id);
        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        return employee;
    }

    public async Task<Employee> CreateEmployee(CreateEmployeeDTO createEmployeeDto)
    {
        var employeeExist = await _dbContext.Employees.AnyAsync(x=> x.Email == createEmployeeDto.Email);
        if (employeeExist)
        {
            throw new Exception($"Employee with email {createEmployeeDto.Email} already exists");
        }
        
        
        
        //Turn the input DTO into a valid Database Entity
        var newEmployee = new Employee
        {
            FullName = createEmployeeDto.FullName,
            Email = createEmployeeDto.Email,
            Department = createEmployeeDto.Department,
            DateJoined = createEmployeeDto.DateJoined,
        };
        
        _dbContext.Employees.Add(newEmployee);
        await _dbContext.SaveChangesAsync();
        return newEmployee;
    }
    

    public async Task<Employee> UpdateEmployee(int id, UpdateEmployeeDTO updateEmployeeDto)
    {
        var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x=> x.Id == id);
        if (existingEmployee == null)
        {
            throw new Exception($"Employee with id {id} not found");
        }

        if (existingEmployee.Email != updateEmployeeDto.Email)
        {
            var emailExists = await _dbContext.Employees.AnyAsync(x=> x.Email == updateEmployeeDto.Email);
            if (emailExists)
            {
                throw new Exception($"Employee with email {updateEmployeeDto.Email} already exists");
            }
        }
        
        existingEmployee.FullName = updateEmployeeDto.FullName;
        existingEmployee.Email = updateEmployeeDto.Email;
        existingEmployee.Department = updateEmployeeDto.Department;
        
        await _dbContext.SaveChangesAsync();
        return existingEmployee;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x=> x.Id == id);

        if (employee == null)
        {
            return false;
        }
        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
   
  
    
    
}