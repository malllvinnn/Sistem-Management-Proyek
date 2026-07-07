using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.TaskItem;

namespace SistemManagementProjectAPI.Services;

public interface ITaskItemService
{
    Task<ServiceResult<List<TaskItemDto>>> GetByProjectIdAsync(Guid projectId);
    Task<ServiceResult<TaskItemDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<TaskItemDto>> CreateAsync(CreateTaskItemDto createDto);
    Task<ServiceResult<TaskItemDto>> UpdateAsync(Guid id, UpdateTaskItemDto updateDto);
    Task<ServiceResult<object>> DeleteAsync(Guid id);
}