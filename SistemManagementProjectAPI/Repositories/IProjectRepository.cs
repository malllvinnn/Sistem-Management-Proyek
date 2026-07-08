using SistemManagementProjectAPI.Models;
using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllActiveAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<bool> ExistsByTitleAsync(string title);
    Task<List<Project>> GetFilteredAsync(string? searchTitle, ProjectStatus? status);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeactivateAsync(Guid id);
    Task<bool> AssignDeveloperAsync(Guid projectId, Guid developerId);
    Task<bool> UnassignDeveloperAsync(Guid projectId, Guid developerId);
}