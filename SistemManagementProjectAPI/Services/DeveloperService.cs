using AutoMapper;
using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Developer;
using SistemManagementProjectAPI.Models;
using SistemManagementProjectAPI.Repositories;

namespace SistemManagementProjectAPI.Services;

public class DeveloperService : IDeveloperService
{
    private readonly IDeveloperRepository _developerRepository;
    private readonly IMapper _mapper;

    public DeveloperService(IDeveloperRepository developerRepository, IMapper mapper)
    {
        _developerRepository = developerRepository;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<List<DeveloperDto>>> GetAllAsync()
    {
        var developers = await _developerRepository.GetAllAsync();
        var dtos = _mapper.Map<List<DeveloperDto>>(developers);
        var responseSuccess = ServiceResult<List<DeveloperDto>>.Success(dtos);

        return responseSuccess;
    }

    public async Task<ServiceResult<DeveloperDto>> GetByIdAsync(Guid id)
    {
        var developer = await _developerRepository.GetByIdAsync(id);

        if (developer == null)
        {
            var responseFailure = ServiceResult<DeveloperDto>.Failure("Developer not found");
            
            return responseFailure;
        }

        var dto = _mapper.Map<DeveloperDto>(developer);
        var responseSuccess = ServiceResult<DeveloperDto>.Success(dto);

        return responseSuccess;
    }

    public async Task<ServiceResult<DeveloperDto>> CreateAsync(CreateDeveloperDto createDto)
    {
        var developer = new Developer
        {
            Name = createDto.Name,
            Skill = createDto.Skill
        };

        var created = await _developerRepository.CreateAsync(developer);
        var dto = _mapper.Map<DeveloperDto>(created);
        var responseSuccess = ServiceResult<DeveloperDto>.Success(dto, "Developer created");

        return responseSuccess;
    }

    public async Task<ServiceResult<DeveloperDto>> UpdateAsync(Guid id, UpdateDeveloperDto updateDto)
    {
        var developer = await _developerRepository.GetByIdAsync(id);

        if (developer == null)
        {
            var responseFailure = ServiceResult<DeveloperDto>.Failure("Developer not found");
            
            return responseFailure;
        }

        _mapper.Map(updateDto, developer);

        var updated = await _developerRepository.UpdateAsync(developer);
        var dto = _mapper.Map<DeveloperDto>(updated);
        var responseSuccess = ServiceResult<DeveloperDto>.Success(dto, "Developer updated");

        return responseSuccess;
    }

    public async Task<ServiceResult<object>> DeleteAsync(Guid id)
    {
        var isDeleted = await _developerRepository.DeleteAsync(id);

        if (!isDeleted)
        {
            var responseFailure = ServiceResult<object>.Failure("Developer not found");
            
            return responseFailure;
        }

        var responseSuccess = ServiceResult<object>.Success(new { }, "Developer deleted");
        
        return responseSuccess;
    }
}