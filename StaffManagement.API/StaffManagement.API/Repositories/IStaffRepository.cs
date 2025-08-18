using StaffManagement.API.DTOs;
using StaffManagement.API.Models;

namespace StaffManagement.API.Repositories
{
    public interface IStaffRepository
    {
        Task<ApiResponse<IEnumerable<Staff>>> GetAllAsync();
        Task<ApiResponse<Staff?>> GetByIdAsync(string staffId);
        Task<ApiResponse<Staff>> AddAsync(Staff staff);
        Task<ApiResponse<Staff>> UpdateAsync(Staff staff);
        Task<ApiResponse<bool>> DeleteAsync(string staffId);
        Task<ApiResponse<IEnumerable<Staff>>> SearchAsync(StaffSearchDto search);

    }
}
