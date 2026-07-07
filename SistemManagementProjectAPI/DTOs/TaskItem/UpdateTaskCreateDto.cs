namespace SistemManagementProjectAPI.DTOs.TaskItem;

public class UpdateTaskCreateDto
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}