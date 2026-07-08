using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Repositories;

public interface ITaskItemRepository
{
    Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId);
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<TaskItem> CreateAsync(TaskItem taskItem);
    Task<TaskItem> UpdateAsync(TaskItem taskItem);
    Task<bool> DeleteAsync(Guid id);
}