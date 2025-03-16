using Application.Common.Enums;

namespace Application.Common.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse(ETypeApiResponse apiResponseCode, string message)
        {
            ApiResponseMessage = apiResponseCode.ToString();
            Message = message;
        }

        public ApiResponse(ETypeApiResponse apiResponseCode, T data)
        {
            ApiResponseMessage = apiResponseCode.ToString();
            Data = data;
        }

        public ApiResponse(ETypeApiResponse apiResponseCode, T data, string message)
        {
            ApiResponseMessage = apiResponseCode.ToString();
            Data = data;
        }

        public T Data { get; set; }
        public string ApiResponseMessage { get; set; }
        public string Message { get; set; }
    }
}
