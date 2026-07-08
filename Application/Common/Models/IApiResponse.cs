namespace Application.Common.Models
{
    public interface IApiResponse
    {
        string ApiResponseMessage { get; set; }
        string Message { get; set; }
    }
}
