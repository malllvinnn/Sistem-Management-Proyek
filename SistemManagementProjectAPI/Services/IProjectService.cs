using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Project;
using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.Services;

public interface IProjectService
{
    Task<ServiceResult<List<ProjectDto>>> GetAllAsync();
    Task<ServiceResult<ProjectDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<List<ProjectDto>>> GetFilteredAsync(string? searchTitle, ProjectStatus? status);
    Task<ServiceResult<ProjectDto>> CreateAsync(CreateProjectDto createDto);
    Task<ServiceResult<ProjectDto>> UpdateAsync(Guid id, UpdateProjectDto updateDto);
    Task<ServiceResult<object>> DeactivateAsync(Guid id);
    Task<ServiceResult<ProjectDto>> AssignDeveloperAsync(Guid projectId, Guid developerId);
    Task<ServiceResult<ProjectDto>> UnassignDeveloperAsync(Guid projectId, Guid developerId);
}