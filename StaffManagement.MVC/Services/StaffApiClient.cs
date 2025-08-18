using StaffManagement.MVC.DTOs;
using StaffManagement.MVC.Models;

namespace StaffManagement.MVC.Services
{
    public class StaffApiClient : IStaffApiClient
    {
        private readonly HttpClient _httpClient;
        public StaffApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<IEnumerable<StaffViewModel>>> GetAllAsync()
        {
            try
            {
                string endPoint = "/api/staff/getall";
                var result = await _httpClient.GetFromJsonAsync<ApiResponse<IEnumerable<StaffViewModel>>>(endPoint);
                return result ?? ApiResponse<IEnumerable<StaffViewModel>>.Fail("No response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StaffViewModel>>.Fail(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<StaffViewModel>> GetByIdAsync(string id)
        {
            try
            {
                string endPoint = $"/api/staff/getstaffbyid/{id}";
                var result = await _httpClient.GetFromJsonAsync<ApiResponse<StaffViewModel>>(endPoint);
                return result ?? ApiResponse<StaffViewModel>.Fail("No response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<StaffViewModel>.Fail(ex.Message, 500);
            }

        }
        public async Task<ApiResponse<StaffViewModel>> CreateAsync(StaffViewModel staff)
        {
            try
            {
                string endPoint = "/api/staff/addnew";
                var response = await _httpClient.PostAsJsonAsync(endPoint, staff);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<StaffViewModel>.Fail($"Failed to create staff. Status: {response.StatusCode}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<StaffViewModel>>();
                return result ?? ApiResponse<StaffViewModel>.Fail("Invalid response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<StaffViewModel>.Fail(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            try
            {
                string endPoint = $"/api/staff/delete/{id}";
                var response = await _httpClient.DeleteAsync(endPoint);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<bool>.Fail($"Failed to delete staff. Status: {response.StatusCode}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                return result ?? ApiResponse<bool>.Fail("Invalid response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message, 500);
            }

        }
        public async Task<ApiResponse<IEnumerable<StaffViewModel>>> SearchAsync(StaffSearchDto searchDto)
        {
            try
            {
                string endPoint = "/api/staff/search";
                var response = await _httpClient.PostAsJsonAsync(endPoint, searchDto);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<IEnumerable<StaffViewModel>>.Fail($"Failed to search staff. Status: {response.StatusCode}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<StaffViewModel>>>();
                return result ?? ApiResponse<IEnumerable<StaffViewModel>>.Fail("Invalid response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StaffViewModel>>.Fail(ex.Message, 500);
            }
        }

        public async Task<ApiResponse<StaffViewModel>> UpdateAsync(string id, StaffViewModel staff)
        {
            try
            {
                string endPoint = "/api/staff/update/";
                var response = await _httpClient.PutAsJsonAsync(endPoint + id, staff);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<StaffViewModel>.Fail($"Failed to update staff. Status: {response.StatusCode}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadFromJsonAsync<ApiResponse<StaffViewModel>>();
                return result ?? ApiResponse<StaffViewModel>.Fail("Invalid response from server");
            }
            catch (Exception ex)
            {
                return ApiResponse<StaffViewModel>.Fail(ex.Message, 500);
            }
        }

        public async Task<byte[]> ExportToExcelAsync()
        {
            string endPoint = "api/staff/export/excel";
            var response = await _httpClient.GetAsync(endPoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to download Excel file.");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> ExportToPdfAsync()
        {
            string endPoint = "api/staff/export/pdf";
            var response = await _httpClient.GetAsync(endPoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to download Pdf file.");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
