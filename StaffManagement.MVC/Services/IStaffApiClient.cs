using StaffManagement.MVC.DTOs;
using StaffManagement.MVC.Models;

namespace StaffManagement.MVC.Services
{
    public interface IStaffApiClient
    {
        Task<ApiResponse<IEnumerable<StaffViewModel>>> GetAllAsync();
        Task<ApiResponse<StaffViewModel>> GetByIdAsync(string id);
        Task<ApiResponse<StaffViewModel>> CreateAsync(StaffViewModel staff);
        Task<ApiResponse<StaffViewModel>> UpdateAsync(string id, StaffViewModel staff);
        Task<ApiResponse<bool>> DeleteAsync(string id);
        Task<ApiResponse<IEnumerable<StaffViewModel>>> SearchAsync(StaffSearchDto searchDto);
        Task<byte[]> ExportToExcelAsync();
        Task<byte[]> ExportToPdfAsync();
    }
}
