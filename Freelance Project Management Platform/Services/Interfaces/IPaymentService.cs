using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentDto>> CreatePayment(int ProjectId, AddPayment request);
        Task<ApiResponse<string>> ConfirmPayment(int paymentId);
        Task<ApiResponse<List<PaymentDto>>> GetProjectPayments(int projectId);
    }
}
