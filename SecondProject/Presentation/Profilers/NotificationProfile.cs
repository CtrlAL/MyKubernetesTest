using AutoMapper;
using NotificationService.Dtos;
using TaskService;

namespace NotificationService.Profilers
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            //Source -> Target
            CreateMap<GrpsTaskModel, ReadTaskDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaskId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
