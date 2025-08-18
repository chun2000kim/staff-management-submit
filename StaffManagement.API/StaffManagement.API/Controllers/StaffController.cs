using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using StaffManagement.API.DTOs;
using StaffManagement.API.Models;
using StaffManagement.API.Repositories;

namespace StaffManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : BaseApiController
    {
        private readonly IStaffRepository _repository;
        public StaffController(IStaffRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _repository.GetAllAsync();
            return ApiResponse(response);
        }

        [HttpGet("getstaffbyid/{staffId}")]
        public async Task<IActionResult> GetStaffById(string staffId)
        {
            var response = await _repository.GetByIdAsync(staffId);
            return ApiResponse(response);
        }

        [HttpPost("addnew")]
        public async Task<IActionResult> Add([FromBody] StaffDto staffDto)
        {
            var staff = new Staff
            {
                StaffId = staffDto.StaffId,
                FullName = staffDto.FullName,
                Birthday = staffDto.Birthday,
                Gender = staffDto.Gender
            };

            var response = await _repository.AddAsync(staff);
            return ApiResponse(response);
        }

        [HttpPut("update/{staffId}")]
        public async Task<IActionResult> Update(string staffId, [FromBody] StaffDto dto)
        {
            // Map DTO to Staff object
            var staffToUpdate = new Staff
            {
                StaffId = dto.StaffId,
                FullName = dto.FullName,
                Birthday = dto.Birthday,
                Gender = dto.Gender
            };

            var response = await _repository.UpdateAsync(staffToUpdate);
            return ApiResponse(response);
        }

        [HttpDelete("delete/{staffId}")]
        public async Task<IActionResult> Delete(string staffId)
        {
            var response = await _repository.DeleteAsync(staffId);
            return ApiResponse(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] StaffSearchDto staffDto)
        {
            var response = await _repository.SearchAsync(staffDto);
            return StatusCode(response.Code, response);
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var response = await _repository.GetAllAsync();
            var staffData = response.Data;
            using var workbook = new XLWorkbook();
            var workSheet = workbook.Worksheets.Add("Staff List");

            // Header
            workSheet.Cell(1, 1).Value = "Staff ID";
            workSheet.Cell(1, 2).Value = "Full Name";
            workSheet.Cell(1, 3).Value = "Birthday";
            workSheet.Cell(1, 4).Value = "Gender";

            // Data 
            int row = 2;
            foreach (var staff in staffData)
            {
                workSheet.Cell(row, 1).Value = staff.StaffId;
                workSheet.Cell(row, 2).Value = staff.FullName;
                workSheet.Cell(row, 3).Value = staff.Birthday.ToString("yyyy-MM-dd");
                workSheet.Cell(row, 4).Value = staff.Gender == 1 ? "Male" : "Female";
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "StaffList.xlsx");
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportToPdf()
        {
            var response = await _repository.GetAllAsync();
            var staffData = response.Data;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text("Staff List").FontSize(20).Bold().AlignCenter();

                    page.Content().PaddingTop(20).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Text("Staff ID").Bold();
                            header.Cell().Text("Full Name").Bold();
                            header.Cell().Text("Birthday").Bold();
                            header.Cell().Text("Gender").Bold();
                        });

                        // Data
                        foreach (var staff in staffData)
                        {
                            table.Cell().BorderBottom(1).BorderColor("#ddd").Padding(5).Text(staff.StaffId);
                            table.Cell().BorderBottom(1).BorderColor("#ddd").Padding(5).Text(staff.FullName);
                            table.Cell().BorderBottom(1).BorderColor("#ddd").Padding(5).Text(staff.Birthday.ToString("yyyy-MM-dd"));
                            table.Cell().BorderBottom(1).BorderColor("#ddd").Padding(5).Text(staff.Gender == 1 ? "Male" : "Female");
                        }
                    });
                });
            });

            var pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", "StaffList.pdf");

        }

    }
}
