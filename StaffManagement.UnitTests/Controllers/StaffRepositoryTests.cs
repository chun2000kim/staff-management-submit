using Microsoft.EntityFrameworkCore;
using StaffManagement.API.Data;
using StaffManagement.API.Models;
using StaffManagement.API.Repositories;
namespace StaffManagement.UnitTests.Controllers
{
    public class StaffRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly StaffRepository _repository;

        public StaffRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // fresh DB for each test
           .Options;

            _context = new AppDbContext(options);
            _repository = new StaffRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddStaff()
        {
            var staff = new Staff
            {
                StaffId = "SF0005",
                FullName = "John Doe",
                Gender = 1,
                Birthday = DateOnly.FromDateTime(DateTime.Now)
            };

            var result = await _repository.AddAsync(staff);

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Status);
            Assert.Equal("SF0005", result.Data!.StaffId);
            Assert.Single(_context.Staffs);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStaff_WhenExists()
        {
            var staff = new Staff
            {
                StaffId = "SF0005",
                FullName = "Jane",
                Gender = 2,
                Birthday = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync("SF0005");

            Assert.Equal(200, result.Code);
            Assert.Equal("Success", result.Status);
            Assert.NotNull(result.Data);
            Assert.Equal("Jane", result.Data!.FullName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFail_WhenNotFound()
        {
            var result = await _repository.DeleteAsync("SF0005");

            Assert.Equal(404, result.Code);
            Assert.Equal("Error", result.Status);
            Assert.False(result.Data);
        }
    }
}
