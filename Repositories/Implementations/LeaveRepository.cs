using EmployeeLeaveManagementSystem.Data;
using EmployeeLeaveManagementSystem.DTO;
using EmployeeLeaveManagementSystem.Models;
using EmployeeLeaveManagementSystem.Repositories.Interfaces;
using EmployeeLeaveManagementSystem.Exceptions; // 🚀 ADDED THIS IMPORT
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
       var leaveRequests = await _dbContext.LeaveRequests
               .Include(l => l.Employee) 
               .OrderByDescending(l => l.DateCreated)
               .ToListAsync();

        return leaveRequests;
    }

    public async Task<LeaveRequest> GetleaveRequestsbyId(int leaveRequestId)
    {
        var leaveRequest = await _dbContext.LeaveRequests
            .Include(lr=> lr.Employee)
            .FirstOrDefaultAsync(x=> x.Id == leaveRequestId);
        if (leaveRequest == null)
        {
            throw new AppException($"Leave request with id {leaveRequestId} not found.");
        }

        return leaveRequest;
    }

    public async Task<LeaveRequest> SubmitLeaveRequest(SubmitLeaveRequestDto submitLeaveRequestDto)
    {
        
        var employeeExists = await _dbContext.Employees.AnyAsync(x=>x.Id == submitLeaveRequestDto.EmployeeId);
        if (!employeeExists)
        {
            throw new AppException($"Employee with id {submitLeaveRequestDto.EmployeeId} does not exist.");
        }

        if (submitLeaveRequestDto.StartDate > submitLeaveRequestDto.EndDate)
        {
            throw new AppException($"Start date cannot be after end date.");
        }
        
        var hasOverlap = await _dbContext.LeaveRequests
            .AnyAsync(lr => lr.EmployeeId == submitLeaveRequestDto.EmployeeId && 
                            lr.Status != "Rejected" &&
                            submitLeaveRequestDto.StartDate <= lr.EndDate &&
                            submitLeaveRequestDto.EndDate >= lr.StartDate
                            );
        if (hasOverlap)
        {
            throw new AppException("Cannot submit request: The selected dates overlap with an existing leave request.");
        }

        var newLeaveRequest = new LeaveRequest
        {
            EmployeeId = submitLeaveRequestDto.EmployeeId,
            LeaveType = submitLeaveRequestDto.LeaveType,
            StartDate = submitLeaveRequestDto.StartDate,
            EndDate = submitLeaveRequestDto.EndDate,
            Reason = submitLeaveRequestDto.Reason,
            Status = "Pending",
            DateCreated = DateTime.UtcNow,
            Approvals = new List<LeaveApproval>()
        };

        _dbContext.LeaveRequests.Add(newLeaveRequest);
        await _dbContext.SaveChangesAsync();

        return newLeaveRequest;
    }

    public async Task<LeaveRequest> UpdateLeaveRequest(int id, SubmitLeaveRequestDto submitLeaveRequestDto)
    {
        var existingRequest = await _dbContext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRequest == null)
        {
            throw new AppException($"Leave request with id {id} does not exist.");
        }

        if (existingRequest.Status != "Pending")
        {
            throw new AppException("Only pending requests can be updated.");
        }

        if (submitLeaveRequestDto.StartDate > submitLeaveRequestDto.EndDate)
        {
            throw new AppException("Start date cannot be after end date.");
        }
        
        var hasOverlap = await _dbContext.LeaveRequests
            .AnyAsync(lr => lr.EmployeeId == existingRequest.EmployeeId && 
                            lr.Id != id && 
                            lr.Status != "Rejected" &&
                            submitLeaveRequestDto.StartDate <= lr.EndDate &&
                            submitLeaveRequestDto.EndDate >= lr.StartDate
            );

        if (hasOverlap)
        {
            throw new AppException("Cannot update request: The new dates overlap with another existing leave request.");
        }

        existingRequest.LeaveType = submitLeaveRequestDto.LeaveType;
        existingRequest.StartDate = submitLeaveRequestDto.StartDate;
        existingRequest.EndDate = submitLeaveRequestDto.EndDate;
        existingRequest.Reason = submitLeaveRequestDto.Reason;
        
        await _dbContext.SaveChangesAsync();
        return existingRequest;
    }

    public async Task<bool> DeleteLeaveRequest(int leaveRequestId)
    {
        var leaveRequest = await _dbContext.LeaveRequests.FindAsync(leaveRequestId);

        if (leaveRequest == null)
        {
            throw new AppException("Leave request not found.");
        }

        if (leaveRequest.Status != "Pending")
        {
            throw new AppException($"Cannot delete this leave request because it has already been {leaveRequest.Status}.");
        }

        _dbContext.LeaveRequests.Remove(leaveRequest);
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<IEnumerable<LeaveRequest>> GetEmployeeLeaveHistory(int employeeId)
    {
        var employeeExists = await _dbContext.Employees.AnyAsync(x => x.Id == employeeId);
        if (!employeeExists)
        {
            throw new AppException($"Employee with id {employeeId} does not exist.");
        }

        return await _dbContext.LeaveRequests
            .Where(lr => lr.EmployeeId == employeeId)
            .OrderByDescending(lr => lr.StartDate)
            .ToListAsync();
    }
    
    public async Task<LeaveRequest> ApproveLeaveRequest(int id, LeaveActionRequestDto leaveActionRequestDto)
    {
        var leaveRequest = await _dbContext.LeaveRequests.Include(lr => lr.Approvals)
                .FirstOrDefaultAsync(x => x.Id == id);
        if (leaveRequest == null)
        {
            throw new AppException($"Leave request not found.");
        }

        if (leaveRequest.Status == "Approved" || leaveRequest.Status == "Rejected")
        {
            throw new AppException($"Cannot action this request: It is already finalized as {leaveRequest.Status}");
        }

        if (leaveRequest.EmployeeId == leaveActionRequestDto.ApproverId)
        {
            throw new AppException("You cannot approve your own leave request.");
        }

        var alreadyActed = leaveRequest.Approvals
            .Any(a => a.ApproverId == leaveActionRequestDto.ApproverId);

        if (alreadyActed)
        {
            throw new AppException("Approver already acted on this request.");
        }
        
        leaveRequest.Approvals.Add(new LeaveApproval
        {
            LeaveRequestId =  id,
            ApproverId = leaveActionRequestDto.ApproverId,
            Action = "Approved",
            Reason = leaveActionRequestDto.Reason,
            DateActed = DateTime.UtcNow
        });
        
        var approveCount = leaveRequest.Approvals.Count(a=>a.Action == "Approved");

        if (approveCount == 1)
        {
            leaveRequest.Status = "Processing";
        } 
        else if (approveCount == 2)
        {
            leaveRequest.Status = "Approved";
        }
        
        await _dbContext.SaveChangesAsync();
        return leaveRequest;
    }

    public async Task<LeaveRequest> RejectLeaveRequest(int id, LeaveActionRequestDto leaveActionRequestDto)
    {
        var leave = await _dbContext.LeaveRequests
            .Include(lr => lr.Approvals)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (leave == null)
        {
            throw new AppException("Leave request not found.");
        }
        
        if (leave.Status == "Approved" || leave.Status == "Rejected")
        {
            throw new AppException($"Cannot action this request: It is already finalized as {leave.Status}");
        }

        if (leave.EmployeeId == leaveActionRequestDto.ApproverId)
        {
            throw new AppException("You cannot reject your own leave request.");
        }

        var alreadyActed = leave.Approvals.Any(a => a.ApproverId == leaveActionRequestDto.ApproverId);

        if (alreadyActed)
        {
            throw new AppException("You already acted on this request.");
        }

        if (string.IsNullOrWhiteSpace(leaveActionRequestDto.Reason))
        {
            throw new AppException("A rejection reason must be provided.");
        }

        leave.Approvals.Add(new LeaveApproval
        {
            LeaveRequestId = id,
            ApproverId = leaveActionRequestDto.ApproverId,
            Action = "Rejected",
            Reason = leaveActionRequestDto.Reason,
            DateActed = DateTime.UtcNow
        });

        leave.Status = "Rejected";

        await _dbContext.SaveChangesAsync();
        return leave;
    }
    
    public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByStatus(string status)
    {
        return await _dbContext.LeaveRequests
            .Include(lr => lr.Employee)
            .Include(lr=>lr.Approvals)
            .Where(lr => lr.Status.ToUpper() == status.ToUpper())
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Employee>> GetEmployeesCurrentlyOnLeave()
    {
        var today = DateTime.UtcNow.Date;
        
        return await _dbContext.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Status == "Approved" && today >= lr.StartDate.Date &&
                         today <= lr.EndDate.Date).Select(lr => lr.Employee)
            .Distinct().ToListAsync();
    }
    
    public async Task<Dictionary<string, int>> GetLeaveStatisticsByDepartment()
    {
        return await _dbContext.LeaveRequests.Include(lr=>lr.Employee)
            .Where(lr => lr.Status == "Approved")
            .GroupBy(lr => lr.Employee.Department)
            .ToDictionaryAsync(group => group.Key ?? "Unknown", group => group.Count());
    }
}
