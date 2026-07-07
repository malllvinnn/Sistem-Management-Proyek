namespace SistemManagementProjectAPI.Models;

public class TaskItem
{
    public Guid Id { get; set; } // Primary key
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    
    public int ProjectId { get; set; } // Foregn key
    public Project Project { get; set; } = null!; // Navigation property -> Project
}