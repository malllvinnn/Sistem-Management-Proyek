namespace SistemManagementProyek.Models;

public class TaskItem
{
    public int Id { get; set; } // Primary key
    public string Title { get; set; }
    public bool IsCompleted { get; set; } = false;
    
    public int ProjectId { get; set; } // Foregn key
    public Project Project { get; set; } // Navigation property -> Project
}