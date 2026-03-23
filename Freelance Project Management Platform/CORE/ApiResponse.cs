using System.Net;
namespace Freelance_Project_Management_Platform.CORE
{
    public class ApiResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
    }
}
