namespace StaffManagement.MVC.Models
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public int? TotalRecords { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string? message = null, int code = 200, int? totalRecords = null)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Status = "success",
                Message = message,
                TotalRecords = totalRecords,
                Data = data
            };
        }

        public static ApiResponse<T> Fail(string message, int code = 400, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Code = code,
                Status = "fail",
                Message = message,
                Errors = errors
            };
        }
    }
}
