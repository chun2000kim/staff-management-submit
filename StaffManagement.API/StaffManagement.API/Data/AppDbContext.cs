using Microsoft.EntityFrameworkCore;
using StaffManagement.API.Models;

namespace StaffManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Staff> Staffs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>().HasData(
                new Staff()
                {
                    StaffId = "SF00001",
                    FullName = "Staff 1",
                    Birthday = new DateOnly(2000, 01, 01),
                    Gender = 1
                },
                new Staff()
                {
                    StaffId = "SF00002",
                    FullName = "Staff 2",
                    Birthday = new DateOnly(1995, 05, 30),
                    Gender = 2
                }


            );
        }
    }
}
