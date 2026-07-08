using Microsoft.AspNetCore.Identity;

namespace SistemManagementProjectAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}