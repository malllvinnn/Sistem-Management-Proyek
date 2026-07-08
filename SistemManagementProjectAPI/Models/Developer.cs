namespace SistemManagementProjectAPI.Models;

public class Developer
{
    public Guid Id { get; set; } // Primary key
    public string Name { get; set; } = string.Empty;
    public string Skill { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Project> Projects { get; set; } = new List<Project>(); // many-to-many
}