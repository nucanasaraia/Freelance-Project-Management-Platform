using Freelance_Project_Management_Platform.CORE;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ApiResponse<string>> SendMessage(int receiverId, AddMessage request);
        Task<ApiResponse<List<MessageDto>>> GetConversation(int otherUserId);
    }
}
