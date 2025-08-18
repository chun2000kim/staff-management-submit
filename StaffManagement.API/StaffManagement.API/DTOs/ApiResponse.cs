namespace StaffManagement.API.DTOs
{
    public class ApiResponse<T>
    {
        public int Code { get; set; } = 200;
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = string.Empty;
        public int? TotalRecords { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "", int? totalRecords = null)
        {
            return new ApiResponse<T>
            {
                Code = 200,
                Status = "Success",
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
                Status = "Error",
                Message = message,
                Errors = errors
            };
        }

    }
}
