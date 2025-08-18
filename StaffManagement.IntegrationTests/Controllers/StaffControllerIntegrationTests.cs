using Microsoft.AspNetCore.Mvc.Testing;
using StaffManagement.API.DTOs;
using StaffManagement.API.Models;
using System.Net.Http.Json;

namespace StaffManagement.IntegrationTests.Controllers
{
    public class StaffControllerIntegrationTests:IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StaffControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();    
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("/api/staff/getall");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<Staff>>>();

            Assert.NotNull(result);
            Assert.Equal(200, result!.Code);
            Assert.Equal("Success", result.Status);
        }

        [Fact]
        public async Task AddNew_ShouldReturnCreated()
        {
            var staff = new
            {
                StaffId = "SF0007",
                FullName = "Staff 7",
                Gender = 2,
                Birthday = DateOnly.Parse("1999-01-01")
            };

            var response = await _client.PostAsJsonAsync("/api/staff/addnew", staff);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Staff>>();

            Assert.NotNull(result);
            Assert.Equal(200, result!.Code);
            Assert.Equal("Success", result.Status);
            Assert.Equal("Staff 7", result.Data!.FullName);
        }
    }
}
