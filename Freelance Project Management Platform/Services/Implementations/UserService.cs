using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Enum;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<string>> BanUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                return ApiResponseFactory.NotFound<string>("user not found");
            }

            if (!user.IsActive)
                return ApiResponseFactory.BadRequest<string>("user is already banned");

            if (user.Role == USER_ROLE.ADMIN)
                return ApiResponseFactory.BadRequest<string>("cannot ban admin");

            user.IsActive = false;
            await _context.SaveChangesAsync();

            return ApiResponseFactory.Success("user has been banned");
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            var result = _mapper.Map<List<UserDto>>(users);
            return ApiResponseFactory.Success(result);
        }
    }
}
