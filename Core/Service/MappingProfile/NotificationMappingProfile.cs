using AutoMapper;
using Domain.Entites.Notifications;
using Shared.DTOs.NotificationsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfile
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationResponseDTO>();
        }
    }
}
