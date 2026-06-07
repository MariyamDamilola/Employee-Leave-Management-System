
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<LeaveRequest>()
            .HasOne(lr => lr.Employee) // A LeaveRequest has One Employee
            .WithMany(e => e.LeaveRequests) // An Employee has Many LeaveRequests
            .HasForeignKey(lr => lr.EmployeeId); // The Foreign Key is EmployeeId
    }
}