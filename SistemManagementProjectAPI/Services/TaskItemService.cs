using AutoMapper;
using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.TaskItem;
using SistemManagementProjectAPI.Repositories;
using TaskItemEntity = SistemManagementProjectAPI.Models.TaskItem;

namespace SistemManagementProjectAPI.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public TaskItemService(ITaskItemRepository taskItemRepository, IProjectRepository projectRepository, IMapper mapper)
    {
        _taskItemRepository = taskItemRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<List<TaskItemDto>>> GetByProjectIdAsync(Guid projectId)
    {
        var taskItems = await _taskItemRepository.GetByProjectIdAsync(projectId);
        var dtos = _mapper.Map<List<TaskItemDto>>(taskItems);
        var responseSuccess = ServiceResult<List<TaskItemDto>>.Success(dtos);

        return responseSuccess;
    }

    public async Task<ServiceResult<TaskItemDto>> GetByIdAsync(Guid id)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id);

        if (taskItem == null)
        {
            var responseFailure = ServiceResult<TaskItemDto>.Failure("TaskItem not found");
            
            return responseFailure;
        }

        var dto = _mapper.Map<TaskItemDto>(taskItem);
        var responseSuccess = ServiceResult<TaskItemDto>.Success(dto);

        return responseSuccess;
    }

    public async Task<ServiceResult<TaskItemDto>> CreateAsync(CreateTaskItemDto createDto)
    {
        var project = await _projectRepository.GetByIdAsync(createDto.ProjectId);

        if (project == null)
        {
            var responseFailure = ServiceResult<TaskItemDto>.Failure("Project not found");
            
            return responseFailure;
        }

        var taskItem = new TaskItemEntity
        {
            Title = createDto.Title,
            ProjectId = createDto.ProjectId
        };

        var created = await _taskItemRepository.CreateAsync(taskItem);
        var dto = _mapper.Map<TaskItemDto>(created);
        var responseSuccess = ServiceResult<TaskItemDto>.Success(dto, "TaskItem created");

        return responseSuccess;
    }

    public async Task<ServiceResult<TaskItemDto>> UpdateAsync(Guid id, UpdateTaskItemDto updateDto)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id);

        if (taskItem == null)
        {
            var responseFailure = ServiceResult<TaskItemDto>.Failure("TaskItem not found");
            
            return responseFailure;
        }

        _mapper.Map(updateDto, taskItem);

        var updated = await _taskItemRepository.UpdateAsync(taskItem);
        var dto = _mapper.Map<TaskItemDto>(updated);
        var responseSuccess = ServiceResult<TaskItemDto>.Success(dto, "TaskItem updated");

        return responseSuccess;
    }

    public async Task<ServiceResult<object>> DeleteAsync(Guid id)
    {
        var isDeleted = await _taskItemRepository.DeleteAsync(id);

        if (!isDeleted)
        {
            var responseFailure = ServiceResult<object>.Failure("TaskItem not found");
            
            return responseFailure;
        }

        var responseSuccess = ServiceResult<object>.Success(new { }, "TaskItem deleted");
        
        return responseSuccess;
    }
}