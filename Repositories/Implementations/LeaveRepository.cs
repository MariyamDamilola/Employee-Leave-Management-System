using EmployeeLeaveManagementSystem.Data;
using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSystem.Repositories.Implementations;

public class LeaveRepository : ILeaveRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LeaveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<IEnumerable<LeaveRequest>> GetAllleaveRequests()
    {
        var leaveRequest = await _dbContext.LeaveRequests
            .Include(lr=> lr.Employee)
            .OrderByDescending(lr=>lr.DateCreated).ToListAsync();
        return leaveRequest;
    }

    public async Task<LeaveRequest> GetleaveRequestsbyId(int leaveRequestId)
    {
        var leaveRequest = await _dbContext.LeaveRequests
            .Include(lr=> lr.Employee)
            .FirstOrDefaultAsync(x=> x.Id == leaveRequestId);
        return leaveRequest;
    }

    public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int employeeId)
    {
        var employeeExists = await _dbContext.Employees.AnyAsync(x => x.Id == employeeId);
        if (!employeeExists)
        {
            throw new Exception($"Employee with id {employeeId} does not exist.");
        }

        return await _dbContext.LeaveRequests
            .Where(lr => lr.EmployeeId == employeeId)
            .OrderByDescending(lr => lr.StartDate).ToListAsync();
    }

    public async Task<LeaveRequest> SubmitLeaveRequest(CreateLeaveDTO createLeaveDto)
    {
        //Employee must exist before leave request can be submitted
        var employeeExists = await _dbContext.Employees.AnyAsync(x=>x.Id == createLeaveDto.EmployeeId);
        if (!employeeExists)
        {
            throw new Exception($"Employee with id {createLeaveDto.EmployeeId} does not exist.");
        }

        // start day cannot be later than end date
        if (createLeaveDto.StartDate > createLeaveDto.EndDate)
        {
            throw new Exception($"Start date must be before end date.");
        }
        
        var hasOverlap = await _dbContext.LeaveRequests
            .AnyAsync(lr => lr.EmployeeId == createLeaveDto.EmployeeId && lr.Status != "Rejected" &&
                            createLeaveDto.StartDate <= lr.EndDate &&
                            createLeaveDto.EndDate >= lr.StartDate
                            );
        if (hasOverlap)
        {
            throw new Exception("Cannot submit request: The selected dates overlap with an existing leave request. ");
        }

        var newLeaveRequest = new LeaveRequest
        {
            EmployeeId = createLeaveDto.EmployeeId,
            LeaveType = createLeaveDto.LeaveType,
            StartDate = createLeaveDto.StartDate,
            EndDate = createLeaveDto.EndDate,
            Reason = createLeaveDto.Reason,

            Status = "Pending",

            DateCreated = DateTime.Now
        };

        _dbContext.LeaveRequests.Add(newLeaveRequest);
        await _dbContext.SaveChangesAsync();

        return newLeaveRequest;
    }
    

    public async Task<LeaveRequest> UpdateLeaveRequest(int leaveRequestId, CreateLeaveDTO createLeaveDto)
    {
        var existingRequest = await _dbContext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == leaveRequestId);
        if (existingRequest == null)
        {
            throw new Exception($"Leave request with id {leaveRequestId} does not exist.");
        }
        
        if (createLeaveDto.StartDate > createLeaveDto.EndDate)
        {
            throw new Exception($"Start date must be before end date.");
        }
        
        var hasOverlap = await _dbContext.LeaveRequests
            .AnyAsync(lr => lr.EmployeeId == createLeaveDto.EmployeeId && lr.Status != "Rejected" &&
                            createLeaveDto.StartDate <= lr.EndDate &&
                            createLeaveDto.EndDate >= lr.StartDate
            );

        if (hasOverlap)
        {
            throw new Exception("Cannot update request: The new date overlap with another existing leave request.");
        }

        existingRequest.LeaveType = createLeaveDto.LeaveType;
        existingRequest.StartDate = createLeaveDto.StartDate;
        existingRequest.EndDate = createLeaveDto.EndDate;
        existingRequest.Reason = createLeaveDto.Reason;
        
        await _dbContext.SaveChangesAsync();

        return existingRequest;
    }

    public async Task<bool> DeleteLeaveRequest(int leaveRequestId)
    {
        var leaveRequest = await _dbContext.LeaveRequests.FindAsync(leaveRequestId);

        if (leaveRequest == null)
        {
            return false;
        }

        if (leaveRequest.Status != "Pending")
        {
            throw new Exception($"Cnnot delete this leave request because it has already been {leaveRequest.Status}");

        }

        _dbContext.LeaveRequests.Remove(leaveRequest);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    //Reject a Leave Request
    public async Task<LeaveRequest> RejectLeaveRequest(int leaveRequestId)
    {
        var leaveRequest = await _dbContext.LeaveRequests.FindAsync(leaveRequestId);
        if (leaveRequest == null)
        {
            throw new Exception($"Leave request with ID {leaveRequestId} does not exist.");
        }

        if (leaveRequest.Status != "Pending")
        {
            throw new Exception(
                $"Cannot reject request. This leave request is already marked as {leaveRequest.Status}.");
        }

        leaveRequest.Status = "Rejected";
        await _dbContext.SaveChangesAsync();
        return leaveRequest;
    }
    
    public async Task<IEnumerable<LeaveRequest>> FilterLeaveRequestsByStatus(string status)
    {
        return await _dbContext.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Status.ToLower() == status.ToLower())
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave()
    {
        var today = DateTime.UtcNow.Date;

        return await _dbContext.LeaveRequests
            .Where(lr => lr.Status == "Approved" && today >= lr.StartDate.Date && today <= lr.EndDate.Date)
            .Select(lr => lr.Employee)
            .Distinct() // Prevents duplicate employee entries if they have overlapping approved rows
            .ToListAsync();
    }
    
    public async Task<Dictionary<string, int>> GetLeaveStatisticsByDepartment()
    {
        return await _dbContext.LeaveRequests
            .Where(lr => lr.Status == "Approved")
            .GroupBy(lr => lr.Employee.Department)
            .ToDictionaryAsync(group => group.Key ?? "Unknown", group => group.Count());
    }



}