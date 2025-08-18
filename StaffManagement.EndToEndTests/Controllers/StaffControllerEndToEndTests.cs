using Microsoft.AspNetCore.Mvc.Testing;
using StaffManagement.API.DTOs;
using StaffManagement.API.Models;
using System.Net.Http.Json;

namespace StaffManagement.EndToEndTests.Controllers
{
    public class StaffControllerEndToEndTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StaffControllerEndToEndTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

        }

        [Fact]
        public async Task Staff_CRUD_EndToEnd()
        {
            // 1. CREATE staff
            var newStaff = new
            {
                StaffId = "SF0008",
                FullName = "End2End User",
                Gender = 1,
                Birthday = "1999-12-31"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/staff/addnew", newStaff);
            createResponse.EnsureSuccessStatusCode();

            var created = await createResponse.Content.ReadFromJsonAsync<ApiResponse<Staff>>();
            Assert.NotNull(created);
            Assert.Equal("End2End User", created!.Data!.FullName);

            // 2. READ staff by Id
            var getResponse = await _client.GetAsync($"/api/staff/getstaffbyid/{newStaff.StaffId}");
            getResponse.EnsureSuccessStatusCode();

            var fetched = await getResponse.Content.ReadFromJsonAsync<ApiResponse<Staff>>();
            Assert.NotNull(fetched);
            Assert.Equal("SF0008", fetched!.Data!.StaffId);

            // 3. UPDATE staff
            var updatedStaff = new
            {
                StaffId = "SF0008",
                FullName = "End2End User Updated",
                Gender = 2,
                Birthday = "2000-01-01"
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/staff/update/{updatedStaff.StaffId}", updatedStaff);
            updateResponse.EnsureSuccessStatusCode();

            var updated = await updateResponse.Content.ReadFromJsonAsync<ApiResponse<Staff>>();
            Assert.NotNull(updated);
            Assert.Equal("End2End User Updated", updated!.Data!.FullName);

            // 4. DELETE staff
            var deleteResponse = await _client.DeleteAsync($"/api/staff/delete/{newStaff.StaffId}");
            deleteResponse.EnsureSuccessStatusCode();

            var deleted = await deleteResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>();
            Assert.NotNull(deleted);
            Assert.True(deleted!.Data);

            // 5. READ again → should return not found
            var notFoundResponse = await _client.GetAsync($"/api/staff/getstaffbyid/{newStaff.StaffId}");
            var notFound = await notFoundResponse.Content.ReadFromJsonAsync<ApiResponse<Staff>>();
            Assert.Equal("Error", notFound!.Status);
        }
    }
}
