using EmployeeLeaveManagementSystem.Data;
using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
using EmployeeLeaveManagementSystem.Exceptions; // 🚀 1. ADD THIS IMPORT

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
      
        return await _dbContext.Employees.AsNoTracking().ToListAsync();
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (employee == null)
        {
            throw new AppException($"Employee with ID {id} was not found.");
        }

        return employee;
    }

    public async Task<Employee> CreateEmployee(CreateEmployeeRequestDto createEmployeeRequestDto)
    {
        var employeeExist = await _dbContext.Employees.AnyAsync(x => x.Email == createEmployeeRequestDto.Email);
        if (employeeExist)
        {
            throw new AppException($"Employee with email {createEmployeeRequestDto.Email} already exists.");
        }
        
        var newEmployee = new Employee
        {
            FullName = createEmployeeRequestDto.FullName,
            Email = createEmployeeRequestDto.Email,
            Department = createEmployeeRequestDto.Department,
            DateJoined = DateTime.UtcNow
        };
        
        _dbContext.Employees.Add(newEmployee);
        await _dbContext.SaveChangesAsync();
        return newEmployee;
    }

    public async Task<Employee> UpdateEmployee(int id, UpdateEmployeeRequestDto updateEmployeeRequestDto)
    {
        var existingEmployee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        if (existingEmployee == null)
        {
            throw new AppException($"Employee with id {id} not found.");
        }

        if (existingEmployee.Email != updateEmployeeRequestDto.Email)
        {
            var emailExists = await _dbContext.Employees.AnyAsync(x => x.Email == updateEmployeeRequestDto.Email);
            if (emailExists)
            {
                throw new AppException($"Employee with email {updateEmployeeRequestDto.Email} already exists.");
            }
        }
        
        existingEmployee.FullName = updateEmployeeRequestDto.FullName;
        existingEmployee.Email = updateEmployeeRequestDto.Email;
        existingEmployee.Department = updateEmployeeRequestDto.Department;
        
        await _dbContext.SaveChangesAsync();
        return existingEmployee;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            throw new AppException($"Employee with ID {id} not found.");
        }
        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}