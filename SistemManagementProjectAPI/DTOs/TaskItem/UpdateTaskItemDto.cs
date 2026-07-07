namespace SistemManagementProjectAPI.DTOs.TaskItem;

public class UpdateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}