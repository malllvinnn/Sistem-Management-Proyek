using AutoMapper;
using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Project;
using SistemManagementProjectAPI.Models;
using SistemManagementProjectAPI.Models.Enums;
using SistemManagementProjectAPI.Repositories;

namespace SistemManagementProjectAPI.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<List<ProjectDto>>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllActiveAsync();
        var dtos = _mapper.Map<List<ProjectDto>>(projects);
        var responseSuccess = ServiceResult<List<ProjectDto>>.Success(dtos);

        return responseSuccess;
    }

    public async Task<ServiceResult<ProjectDto>> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
        {
            var responseFailure = ServiceResult<ProjectDto>.Failure("Project not found");
            
            return responseFailure;
        }
        
        var dto = _mapper.Map<ProjectDto>(project);
        var responseSuccess = ServiceResult<ProjectDto>.Success(dto);

        return responseSuccess;
    }

    public async Task<ServiceResult<List<ProjectDto>>> GetFilteredAsync(string? searchTitle, ProjectStatus? status)
    {
        var projects = await _projectRepository.GetFilteredAsync(searchTitle, status);
        var dtos = _mapper.Map<List<ProjectDto>>(projects);
        var responseSuccess = ServiceResult<List<ProjectDto>>.Success(dtos);

        return responseSuccess;
    }

    public async Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto createDto)
    {
        var exists = await _projectRepository.ExistsByTitleAsync(createDto.Title);

        if (exists)
        {
            var responseFailure = ServiceResult<ProjectDto>.Failure($"Project Title '{createDto.Title}' already exists");

            return responseFailure;
        }

        var project = new Project
        {
            Title = createDto.Title,
            Status = ProjectStatus.NotStarted
        };
        
        var created = await _projectRepository.CreateAsync(project);
        var dto = _mapper.Map<ProjectDto>(created);
        var responseSuccess = ServiceResult<ProjectDto>.Success(dto, "Project Created");

        return responseSuccess;
    }

    public async Task<ServiceResult<ProjectDto>> UpdateAsync(Guid id, UpdateProjectDto updateDto)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project == null)
        {
            var responseFailure = ServiceResult<ProjectDto>.Failure("Project not found");

            return responseFailure;
        }
        
        _mapper.Map(updateDto, project);
        
        var updated = await _projectRepository.UpdateAsync(project);
        var dto = _mapper.Map<ProjectDto>(updated);
        var responseSuccess = ServiceResult<ProjectDto>.Success(dto, "Project Updated");

        return responseSuccess;
    }

    public async Task<ServiceResult<object>> DeactivateAsync(Guid id)
    {
        var isDeactivated = await _projectRepository.DeactivateAsync(id);

        if (!isDeactivated)
        {
            var responseFailure = ServiceResult<object>.Failure("Project is not deactivated");

            return responseFailure;
        }
        
        var responseSuccess = ServiceResult<object>.Success(new{}, "Project is deactivated");

        return responseSuccess;
    }

    public async Task<ServiceResult<ProjectDto>> AssignDeveloperAsync(Guid projectId, Guid developerId)
    {
        var isAssignedToDeveloper = await _projectRepository.AssignDeveloperAsync(projectId, developerId);

        if (!isAssignedToDeveloper)
        {
            var responseFailure = ServiceResult<ProjectDto>.Failure("Project is not assigned to developer");
            
            return responseFailure;
        }
        
        var projectUpdated = await _projectRepository.GetByIdAsync(projectId);
        var dto = _mapper.Map<ProjectDto>(projectUpdated);
        var responseSuccess = ServiceResult<ProjectDto>.Success(dto, "Project is assigned to developer");

        return responseSuccess;
    }

    public async Task<ServiceResult<ProjectDto>> UnassignDeveloperAsync(Guid projectId, Guid developerId)
    {
        var isUnassignedFromDeveloper = await _projectRepository.UnassignDeveloperAsync(projectId, developerId);

        if (!isUnassignedFromDeveloper)
        {
            var responseFailure = ServiceResult<ProjectDto>.Failure("Project is not unassigned from developer");

            return responseFailure;
        }
        var projectUpdated = await _projectRepository.GetByIdAsync(projectId);
        var dto = _mapper.Map<ProjectDto>(projectUpdated);
        var responseSuccess = ServiceResult<ProjectDto>.Success(dto, "Project is unassigned from developer");

        return responseSuccess;
    }
}