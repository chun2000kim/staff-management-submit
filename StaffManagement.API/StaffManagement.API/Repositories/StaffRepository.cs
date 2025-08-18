using Microsoft.EntityFrameworkCore;
using StaffManagement.API.Data;
using StaffManagement.API.DTOs;
using StaffManagement.API.Models;
using System.Globalization;

namespace StaffManagement.API.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;
        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<Staff>> AddAsync(Staff staff)
        {
            try
            {
                _context.Staffs.Add(staff);
                await _context.SaveChangesAsync();
                return ApiResponse<Staff>.Success(staff, "Staff added successfully");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;

                return ApiResponse<Staff>.Fail(
                    "Failed to add staff",
                    500,
                    new List<string> { errorMessage }
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string staffId)
        {
            try
            {
                var staff = await _context.Staffs.FindAsync(staffId);
                if (staff == null)
                    return ApiResponse<bool>.Fail("Staff not found", 404);

                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, "Staff deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail("Failed to delete staff", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<Staff>>> GetAllAsync()
        {
            try
            {
                var staffList = await _context.Staffs.AsNoTracking().ToListAsync();
                return ApiResponse<IEnumerable<Staff>>.Success(staffList, totalRecords: staffList.Count);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Staff>>.Fail("Failed to get staff list", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<Staff?>> GetByIdAsync(string staffId)
        {
            try
            {
                var staff = await _context.Staffs.AsNoTracking().FirstOrDefaultAsync(s => s.StaffId == staffId);
                if (staff == null)
                    return ApiResponse<Staff?>.Fail("Staff not found", 404);

                return ApiResponse<Staff?>.Success(staff);
            }
            catch (Exception ex)
            {
                return ApiResponse<Staff?>.Fail("Failed to get staff", 500, new List<string> { ex.Message });
            }
        }
        public async Task<ApiResponse<IEnumerable<Staff>>> SearchAsync(StaffSearchDto search)
        {
            try
            {
                string dateFrom = search.BirthdayFrom ?? "1900-01-01";
                string dateTo = search.BirthdayTo ?? "1900-01-01";
                DateOnly birthdayFrom = DateOnly.ParseExact(dateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateOnly birthdayTo = DateOnly.ParseExact(dateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                
                var query = _context.Staffs.AsQueryable()
                                .Where(s => s.StaffId == search.StaffId
                                     || s.Gender == search.Gender
                                     || (s.Birthday >= birthdayFrom && s.Birthday <= birthdayTo)
                                );
                var result = await query.ToListAsync();
                return ApiResponse<IEnumerable<Staff>>.Success(result, totalRecords: result.Count);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<Staff>>.Fail("Search failed", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<Staff>> UpdateAsync(Staff staff)
        {
            try
            {
                // Fetch existing tracked entity
                var existingStaff = await _context.Staffs.FindAsync(staff.StaffId);
                if (existingStaff == null)
                    return ApiResponse<Staff>.Fail("Staff not found", 404);

                // Update properties
                existingStaff.FullName = staff.FullName;
                existingStaff.Birthday = staff.Birthday;
                existingStaff.Gender = staff.Gender;

                await _context.SaveChangesAsync();
                return ApiResponse<Staff>.Success(existingStaff, "Staff updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Staff>.Fail("Failed to update staff", 500, new List<string> { ex.Message });
            }
        }

    }
}
