using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class PaymentService : IPaymentService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly IUserLoggerService _logger;

    public PaymentService(DataContext context, IMapper mapper, ICurrentUserService currentUser, IUserLoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
    }

    public async Task<ApiResponse<PaymentDto>> CreatePayment(int projectId, AddPayment request)
    {
        try
        {
            var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.ClientId == _currentUser.UserId);

            if (project == null)
            {
                _logger.LogWarning(null,"Project not found or access denied");
                return ApiResponseFactory.NotFound<PaymentDto>("Project not found or access denied");
            }

            if (project.AcceptedFreelancerId == null)
            {
                _logger.LogWarning(null,"No freelancer assigned to this project yet");
                return ApiResponseFactory.BadRequest<PaymentDto>("No freelancer assigned to this project yet");
            }

            var payment = _mapper.Map<Payment>(request);
            payment.CreatedAt = DateTime.UtcNow;
            payment.ProjectId = projectId;
            payment.ClientId = _currentUser.UserId;
            payment.FreelancerId = project.AcceptedFreelancerId.Value;

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<PaymentDto>(payment);

            _logger.LogInfo(null,"Payment occurred successfully");
            return ApiResponseFactory.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(null,ex,"Unexpected error occurred during payment");
            return ApiResponseFactory.ServerError<PaymentDto>("Unexpected error occurred during payment");
        }
    }

    public async Task<ApiResponse<string>> ConfirmPayment(int paymentId)
    {
        try
        {
            var payment = await _context.Payments
           .Include(p => p.Project)
           .FirstOrDefaultAsync(p => p.Id == paymentId && p.ClientId == _currentUser.UserId);

            if (payment == null)
            {
                _logger.LogWarning(null, "Payment not found or access denied for UserId: {UserId}", _currentUser.UserId);
                return ApiResponseFactory.NotFound<string>("Payment not found or access denied");
            }

            if (payment.Status == PAYMENT_STATUS.COMPLETED)
            {
                _logger.LogWarning(null,"Payment already confirmed");
                return ApiResponseFactory.BadRequest<string>("Payment already confirmed");
            }

            payment.Status = PAYMENT_STATUS.COMPLETED;
            payment.Project.IsPaid = true;

            await _context.SaveChangesAsync();

            _logger.LogInfo(null, "Payment confirmed successfully for UserId: {UserId}", _currentUser.UserId);
            return ApiResponseFactory.Success("Payment confirmed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(null,ex,"Unexpected error occurred during confirming payment");
            return ApiResponseFactory.ServerError<string>("Unexpected error occurred during confirming payment");
        }
    }

    public async Task<ApiResponse<PagedResult<PaymentDto>>> GetProjectPayments(int projectId, PaginationParams pagination)
    {
        try
        {
            if(pagination.Page <= 0 || pagination.PageSize <= 0 || pagination.PageSize > 50)
            {
                _logger.LogWarning(null,"Invalid pagination parameters: Page {Page}, PageSize {PageSize}", pagination.Page, pagination.PageSize);
                return ApiResponseFactory.BadRequest<PagedResult<PaymentDto>>("Invalid pagination parameters");
            }

            var query =  _context.Payments
                .AsNoTracking()
                .Where(p => p.ProjectId == projectId && p.ClientId == _currentUser.UserId);

            var totalCount = await query.CountAsync();
            var payments = await query
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var result = new PagedResult<PaymentDto>
            {
                Items = _mapper.Map<List<PaymentDto>>(payments),
                TotalCount = totalCount,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };

            _logger.LogInfo(null,"Retrieved {Count} payments for project {ProjectId}", payments.Count, projectId);
            return ApiResponseFactory.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(null,ex,"Unexpected error occurred");
            return ApiResponseFactory.ServerError<PagedResult<PaymentDto>>("Unexpected error occurred");
        }
    }
}