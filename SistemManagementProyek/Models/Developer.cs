namespace SistemManagementProyek.Models;

public class Developer
{
    public int Id { get; set; } // Primary key
    public string Name { get; set; }
    public string Skill { get; set; }

    // Navigation property
    public ICollection<Project> Projects { get; set; } = new List<Project>(); // many-to-many
}