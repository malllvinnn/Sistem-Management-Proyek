using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.Models;

public class Project
{
    public Guid Id { get; set; } // Primary key
    public string Title { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>(); // one-to-many
    public ICollection<Developer> Developers { get; set; } = new List<Developer>(); // many-to-many
}