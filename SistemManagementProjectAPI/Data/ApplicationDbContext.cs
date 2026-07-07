using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Developer> Developers { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        base.OnModelCreating(builder);
        
        // Developer
        builder.Entity<Developer>((entity) =>
        {
            entity.Property((d) => d.Name).IsRequired().HasMaxLength(150);
            entity.Property((d) => d.Skill).IsRequired().HasMaxLength(100);
            entity.HasIndex((d) => d.Name).IsUnique();
        });
        
        // Project
        builder.Entity<Project>((entity) =>
        {
            entity.Property((p) => p.Title).IsRequired().HasMaxLength(200);
            entity.Property((p) => p.Status).HasConversion<string>().HasMaxLength(50);
            entity.HasIndex((p) => p.Title).IsUnique();
            entity.HasIndex((p) => p.IsActive);
            entity.HasIndex((p) => p.Status);
        });
        
        // TaskItem (one-to-many)
        builder.Entity<TaskItem>((entity) =>
        {
            entity.Property((t) => t.Title).IsRequired().HasMaxLength(200);

            entity.HasOne((t) => t.Project)
                .WithMany((p) => p.TaskItems)
                .HasForeignKey((t) => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex((t) => t.ProjectId);
        });
        
        // Project <-> Developer (many-to-many, explicit join table)
        builder.Entity<Project>()
            .HasMany((p) => p.Developers)
            .WithMany((d) => d.Projects)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectDeveloper",
                (j) => j.HasOne<Developer>().WithMany().HasForeignKey("DeveloperId"),
                (j) => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                (j) =>
                {
                    j.HasKey("ProjectId", "DeveloperId");
                    j.ToTable("ProjectDeveloper");
                }
            );
        
        // ApplicationUser
        builder.Entity<ApplicationUser>((entity) =>
        {
            entity.Property((u) => u.FirstName).HasMaxLength(100);
            entity.Property((u) => u.LastName).HasMaxLength(100);
        });
    }
}