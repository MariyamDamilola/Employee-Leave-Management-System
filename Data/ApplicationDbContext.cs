
using EmployeeLeaveManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Employee>Employees { get; set; }
    
    public DbSet<LeaveRequest>LeaveRequests { get; set; }
    
    public DbSet<LeaveApproval>LeaveApprovals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests) 
            .HasForeignKey(lr => lr.EmployeeId);

        modelBuilder.Entity<LeaveApproval>()
            .HasOne(la => la.LeaveRequest)
            .WithMany(lr => lr.Approvals)
            .HasForeignKey(la => la.LeaveRequestId);
        
        modelBuilder.Entity<LeaveApproval>()
            .HasOne(la => la.Approver)
            .WithMany()
            .HasForeignKey(la => la.ApproverId);

    }
}