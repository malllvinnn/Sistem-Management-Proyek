using SistemManagementProjectAPI.DTOs.Developer;
using SistemManagementProjectAPI.DTOs.TaskItem;
using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.DTOs.Project;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public bool IsActive { get; set; }
    
    public List<TaskItemDto> TaskItems { get; set; } = new List<TaskItemDto>();
    public List<DeveloperDto> Developers { get; set; } = new List<DeveloperDto>();
}