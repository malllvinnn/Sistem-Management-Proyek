using Microsoft.EntityFrameworkCore;
using SistemManagementProyek.Models;

namespace SistemManagementProyek.Data;

public class SistemManagementProyekDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<Developer> Developers { get; set; }
    
    public SistemManagementProyekDbContext(DbContextOptions<SistemManagementProyekDbContext> options) : base(options) {}
    public SistemManagementProyekDbContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=SistemManagementProyek.db");
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Project entity
        modelBuilder.Entity<Project>((entity) =>
        {
            entity.HasIndex((p) => p.Title).IsUnique();
        });
        
        // Configure TaskItem entity (One-to-many)
        modelBuilder.Entity<TaskItem>((entity) =>
        {
            entity.HasOne((t) => t.Project)
                .WithMany((p) => p.TaskItems)
                .HasForeignKey((t) => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // configure Developer entity
        modelBuilder.Entity<Developer>((entity) =>
        {
            entity.HasIndex((d) => d.Name).IsUnique();
        });
        
        // Configure Project dan Developer Many-to-many
        modelBuilder.Entity<Project>()
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
    }
}