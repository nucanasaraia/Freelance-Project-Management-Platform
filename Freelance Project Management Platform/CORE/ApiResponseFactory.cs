using System.Net;

namespace Freelance_Project_Management_Platform.CORE
{
    public class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string message = "Request Successful", HttpStatusCode status = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                Status = status,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> Fail<T>(string message, HttpStatusCode status)
        {
            return new ApiResponse<T>
            {
                Status = status,
                Data = default,
                Message = message,
            };
        }


        public static ApiResponse<T> BadRequest<T>(string message = "Bad request") 
            => Fail<T>(message, HttpStatusCode.BadRequest);
        public static ApiResponse<T> NotFound<T>(string message = "Not Found")
            => Fail<T>(message, HttpStatusCode.NotFound);
        public static ApiResponse<T> Unauthorized<T>(string message = "Unauthorized")
           => Fail<T>(message, HttpStatusCode.Unauthorized);
        public static ApiResponse<T> Forbidden<T>(string message = "Forbidden")
            => Fail<T>(message, HttpStatusCode.Forbidden);
        public static ApiResponse<T> Conflict<T>(string message = "Resource already exists")
          => Fail<T>(message, HttpStatusCode.Conflict);
        public static ApiResponse<T> ServerError<T>(string message = "Something went wrong")
            => Fail<T>(message, HttpStatusCode.InternalServerError);
    }
}
