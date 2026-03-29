using AutoMapper;
using TaskService.Dto;
using TaskService.Models;


namespace TaskService.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            //Source -> Target
            CreateMap<CreateTaskModel, Entities.Task>();
            CreateMap<Entities.Task, SendNotificatioDto>();

            CreateMap<Entities.Task, GrpsTaskModel>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Entities.Task, TaskCreatedDto>()
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src => "Task_Created"))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
