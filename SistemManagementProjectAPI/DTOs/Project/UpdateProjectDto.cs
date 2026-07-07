using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.DTOs.Project;

public class UpdateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
}