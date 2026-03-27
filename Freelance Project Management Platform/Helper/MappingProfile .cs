using AutoMapper;
using Freelance_Project_Management_Platform.DTOs;
using Freelance_Project_Management_Platform.Models;
using Freelance_Project_Management_Platform.Request;

namespace Freelance_Project_Management_Platform.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User 
            CreateMap<User, UserDto>();

            //Project 
            CreateMap<AddProject, Project>();
            CreateMap<Project, ProjectDto>();

            //Proposal 
            CreateMap<AddProposal, Proposal>();
            CreateMap<Proposal, ProposalDto>();

            //Task
            CreateMap<AddTask, TaskItem>();
            CreateMap<TaskItem, TaskDto>();

            //Message 
            CreateMap<AddMessage, Message>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderUsername,
                           opt => opt.MapFrom(src => src.Sender.Username))
                .ForMember(dest => dest.ReceiverUsername,
                           opt => opt.MapFrom(src => src.Receiver.Username));

            //Dashboard
            CreateMap<User, ClientDashboardDto>();
            CreateMap<User, FreelancerDashboardDto>();

            //Payment
            CreateMap<AddPayment, Payment>();
            CreateMap<Payment, PaymentDto>();
        }
    }
}
