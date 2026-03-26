using AutoMapper;
using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.Data;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;
using Freelance_Project_Management_Platform.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Freelance_Project_Management_Platform.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public MessageService(DataContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ApiResponse<List<MessageDto>>> GetConversation(int otherUserId)
        {
            var userId = _currentUser.UserId;

            var messages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m =>
                    (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                    (m.SenderId == otherUserId && m.ReceiverId == userId))
                .ToListAsync();

            if (!messages.Any())
            {
                return ApiResponseFactory.Success(new List<MessageDto>());
            }

            var result = _mapper.Map<List<MessageDto>>(messages);
            return ApiResponseFactory.Success(result);
        }

        public async Task<ApiResponse<string>> SendMessage(int receiverId, AddMessage request)
        {
            var message = _mapper.Map<Message>(request);

            message.SenderId = _currentUser.UserId; 
            message.ReceiverId = receiverId;
            message.CreatedAt = DateTime.UtcNow;

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return ApiResponseFactory.Success("Message sent");
        }
    }
}
