using Microsoft.AspNetCore.Mvc;
using StaffManagement.MVC.DTOs;
using StaffManagement.MVC.Models;
using StaffManagement.MVC.Services;

namespace StaffManagement.MVC.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffApiClient _apiClient;
        public StaffController(IStaffApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? staffId, int? gender, DateOnly? birthdayFrom, DateOnly? birthdayTo)
        {
            IEnumerable<StaffViewModel> staffs;

            if (!string.IsNullOrEmpty(staffId) || gender.HasValue || birthdayFrom.HasValue || birthdayTo.HasValue)
            {
                var searchDto = new StaffSearchDto
                {
                    StaffId = staffId,
                    Gender = gender,
                    birthdayFrom = birthdayFrom?.ToString("yyyy-MM-dd"),
                    birthdayTo = birthdayTo?.ToString("yyyy-MM-dd")
                };

                var responseResults = await _apiClient.SearchAsync(searchDto);
                staffs = responseResults.Data ?? new List<StaffViewModel>();
            }
            else
            {
                var response = await _apiClient.GetAllAsync();
                return View(response.Data ?? new List<StaffViewModel>());
            }
            return View(staffs);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(StaffViewModel model)
        {
            TempData["Success"] = string.Empty;
            try
            {
                if (!ModelState.IsValid) return View(model);

                var response = await _apiClient.CreateAsync(model);
                if (response.Status != "Success")
                {
                    TempData["ErrorMessage"] = response.Message;
                    return View(model);
                }
                TempData["Success"] = response.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message.ToString();
                return View(model); 
                
            }

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _apiClient.GetByIdAsync(id);
            if (response.Data == null) return NotFound();
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, StaffViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _apiClient.UpdateAsync(id, model);
            if (response.Status == "Success")
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", response.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _apiClient.GetByIdAsync(id);
            if (response.Data == null) return NotFound();
            return View(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string staffId)
        {
            var response = await _apiClient.GetByIdAsync(staffId);
            if (response.Data == null) return NotFound();
            var result = _apiClient.DeleteAsync(staffId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            var fileBytes = await _apiClient.ExportToExcelAsync();
            return File(fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "StaffList.xlsx");
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf()
        {
            var fileBytes = await _apiClient.ExportToPdfAsync();
            var fileName = "StaffList.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
