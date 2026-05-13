using AutoMapper;
using Domain.Entites.ChatModule;
using Shared.DTOs.ChatDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class ChatMappingProfile : Profile
    {
        public ChatMappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageResponseDTO>();
        }
    }
}
