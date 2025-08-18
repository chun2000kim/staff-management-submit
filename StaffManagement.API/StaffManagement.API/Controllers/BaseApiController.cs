using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffManagement.API.DTOs;

namespace StaffManagement.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ApiResponse<T>(ApiResponse<T> response)
        {
            return response.Code switch
            {
                200 => Ok(response),
                201 => Created("", response),
                400 => BadRequest(response),
                404 => NotFound(response),
                500 => StatusCode(500, response),
                _ => StatusCode(response.Code, response)
            };
        }
    }
}
