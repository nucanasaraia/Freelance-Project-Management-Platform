using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        public UserService(DataContext context, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<string>> BanUser(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    return ApiResponseFactory.NotFound<string>("User not found");

                if (!user.IsActive)
                    return ApiResponseFactory.BadRequest<string>("User is already banned");

                if (user.Role == USER_ROLE.ADMIN)
                    return ApiResponseFactory.BadRequest<string>("Cannot ban admin");

                user.IsActive = false;
                await _context.SaveChangesAsync();

                _logger.LogWarning("User {UserId} has been banned by admin", userId);
                return ApiResponseFactory.Success("User has been banned");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while banning user {UserId}", userId);
                return ApiResponseFactory.ServerError<string>("Unexpected error occurred");
            }
        }
        public async Task<ApiResponse<PagedResult<UserDto>>> GetAllUsers(PaginationParams pagination)
        {
            try
            {
                if (pagination.Page <= 0) pagination.Page = 1;

                var query = _context.Users
                    .AsNoTracking();

                var totalCount = await query.CountAsync();

                var users = await query
                    .OrderBy(u => u.Id) 
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var result = new PagedResult<UserDto>
                {
                    Items = _mapper.Map<List<UserDto>>(users),
                    TotalCount = totalCount,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize
                };

                return ApiResponseFactory.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(null, ex, "Unexpected error in {MethodName}", nameof(GetAllUsers));
                return ApiResponseFactory.ServerError<PagedResult<UserDto>>("Unexpected error occurred");
            }
        }
    }
}
