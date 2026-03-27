using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PaymentService : IPaymentService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public PaymentService(DataContext context, IMapper mapper, ICurrentUserService currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ApiResponse<PaymentDto>> CreatePayment(int projectId, AddPayment request)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == _currentUser.UserId);

        if (project == null)
            return ApiResponseFactory.NotFound<PaymentDto>("Project not found or access denied");

        if (project.AcceptedFreelancerId == null)
            return ApiResponseFactory.BadRequest<PaymentDto>("No freelancer assigned to this project yet");

        var payment = _mapper.Map<Payment>(request);
        payment.CreatedAt = DateTime.UtcNow;
        payment.ProjectId = projectId;
        payment.ClientId = _currentUser.UserId;
        payment.FreelancerId = project.AcceptedFreelancerId.Value;

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<PaymentDto>(payment);
        return ApiResponseFactory.Success(result);
    }

    public async Task<ApiResponse<string>> ConfirmPayment(int paymentId)
    {
        var payment = await _context.Payments
            .Include(p => p.Project)
            .FirstOrDefaultAsync(p => p.Id == paymentId && p.ClientId == _currentUser.UserId);

        if (payment == null)
            return ApiResponseFactory.NotFound<string>("Payment not found or access denied");

        if (payment.Status == PAYMENT_STATUS.COMPLETED)
            return ApiResponseFactory.BadRequest<string>("Payment already confirmed");

        payment.Status = PAYMENT_STATUS.COMPLETED;
        payment.Project.IsPaid = true;

        await _context.SaveChangesAsync();
        return ApiResponseFactory.Success("Payment confirmed successfully");
    }

    public async Task<ApiResponse<List<PaymentDto>>> GetProjectPayments(int projectId)
    {
        var payments = await _context.Payments
            .Where(p => p.ProjectId == projectId && p.ClientId == _currentUser.UserId)
            .ToListAsync();

        if (!payments.Any())
            return ApiResponseFactory.Success(new List<PaymentDto>());

        var result = _mapper.Map<List<PaymentDto>>(payments);
        return ApiResponseFactory.Success(result);
    }
}