using AutoMapper;
using SistemManagementProjectAPI.DTOs.TaskItem;
using SistemManagementProjectAPI.Models;
using TaskItemEntity = SistemManagementProjectAPI.Models.TaskItem;

namespace SistemManagementProjectAPI.MappingProfiles;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<TaskItem, TaskItemDto>()
            .ForMember(dest => dest.ProjectTitle, opt => opt.MapFrom(src => src.Project.Title));
        CreateMap<UpdateTaskItemDto, TaskItemEntity>();
    }
}