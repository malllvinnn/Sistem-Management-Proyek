using SistemManagementProyek.Enums;

namespace SistemManagementProyek.Models;

public class Project
{
    public int Id { get; set; } // Primary key
    public string Title { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
    public bool IsActive { get; set; } = true;
    
    // Navigation Properties
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>(); // one-to-many
    public ICollection<Developer> Developers { get; set; } = new List<Developer>(); // many-to-many
}