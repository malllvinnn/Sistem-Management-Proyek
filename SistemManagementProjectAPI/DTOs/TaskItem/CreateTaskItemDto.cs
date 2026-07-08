namespace SistemManagementProjectAPI.DTOs.TaskItem;

public class CreateTaskItemDto
{
    public string Title { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}