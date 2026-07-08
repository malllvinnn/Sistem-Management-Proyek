using AutoMapper;
using SistemManagementProjectAPI.DTOs.Project;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.MappingProfiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>();
        CreateMap<UpdateProjectDto, Project>();
    }
}