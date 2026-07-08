using AutoMapper;
using SistemManagementProjectAPI.DTOs.Developer;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.MappingProfiles;

public class DeveloperProfile : Profile
{
    public DeveloperProfile()
    {
        CreateMap<Developer, DeveloperDto>();
        CreateMap<UpdateDeveloperDto, Developer>();
    }
}