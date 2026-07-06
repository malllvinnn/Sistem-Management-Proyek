using Microsoft.EntityFrameworkCore;
using SistemManagementProyek.Data;
using SistemManagementProyek.Enums;
using SistemManagementProyek.Models;
using SistemManagementProyek.Services;

namespace SistemManagementProyek;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Sistem Management Proyek Demo ===");

        using var context = new SistemManagementProyekDbContext();
        
        // Apply migration
        await context.Database.MigrateAsync();
        
        // Seed Data
        await SeedDatabaseAsync(context);
        
        // Init Services
        var projectService = new ProjectService(context);
        
        // Demo CRUD
        await DemonstateCrudOperationsAsync(projectService);

    }

    static async Task DemonstateCrudOperationsAsync(ProjectService service)
    {
        Console.WriteLine("\n--- Memulai Demo CRUD Manajemen Proyek ---");
        
        // READ FIRST (Check)
        Console.WriteLine("\n[READ FIRST] Daftar Proyek Aktif Saat Ini:");
        var projectsBeforeAdd = await service.GetAllProjectsAsync();
        foreach (var p in projectsBeforeAdd)
        {
            Console.WriteLine($"- ID: {p.Id} | Judul: {p.Title} | Status: {p.Status}");
            // Menampilkan hasil RELASI
            Console.WriteLine($"  -> Jumlah Developer yang ditugaskan: {p.Developers?.Count ?? 0}");
            Console.WriteLine($"  -> Jumlah Tugas (Tasks): {p.TaskItems?.Count ?? 0}");
        }
        
        // CREATE: Menambah data ke database
        Console.WriteLine("\n[CREATE] Menambahkan Proyek Baru...");
        var newProject = new Project
        {
            Title = "....",
            Status = ProjectStatus.NotStarted
        };
        await service.CreateProjectAsync(newProject);
        Console.WriteLine($"[Sukses] Proyek '{newProject.Title}' berhasil ditambahkan dengan ID {newProject.Id}!");

        // READ
        Console.WriteLine("\n[READ] Daftar Proyek Aktif Saat Ini:");
        var projects = await service.GetAllProjectsAsync();
        foreach (var p in projects)
        {
            Console.WriteLine($"- ID: {p.Id} | Judul: {p.Title} | Status: {p.Status}");
            // Menampilkan hasil RELASI
            Console.WriteLine($"  -> Jumlah Developer yang ditugaskan: {p.Developers?.Count ?? 0}");
            Console.WriteLine($"  -> Jumlah Tugas (Tasks): {p.TaskItems?.Count ?? 0}");
        }
        
        // UPDATE: Mengubah detail relasi
        Console.WriteLine($"\n[UPDATE] Mengubah data Proyek ID {newProject.Id}...");
        // Update teks biasa
        await service.UpdateProjectAsync(newProject.Id, "....", ProjectStatus.InProgress); 
        Console.WriteLine("[Sukses] Nama diubah, status diperbarui");
        
        // READ 
        Console.WriteLine("\n[READ AFTER UPDATE] Daftar Proyek Aktif Saat Ini:");
        var projectsAfterUpdate = await service.GetAllProjectsAsync();
        foreach (var p in projectsAfterUpdate)
        {
            Console.WriteLine($"- ID: {p.Id} | Judul: {p.Title} | Status: {p.Status}");
            // Menampilkan hasil RELASI
            Console.WriteLine($"  -> Jumlah Developer yang ditugaskan: {p.Developers?.Count ?? 0}");
            Console.WriteLine($"  -> Jumlah Tugas (Tasks): {p.TaskItems?.Count ?? 0}");
        }
        
        // DELETE
        Console.WriteLine($"\n[DELETE] Menghapus Proyek ID {newProject.Id} (Soft Delete)...");
        await service.DeactivateProjectAsync(newProject.Id);
        
        // Pembuktian bahwa data disembunyikan, bukan dihapus permanen
        // READ 
        Console.WriteLine("\n[READ AFTER UPDATE] Daftar Proyek Aktif Saat Ini:");
        var projectsAfterDelete = await service.GetAllProjectsAsync();
        foreach (var p in projectsAfterDelete)
        {
            Console.WriteLine($"- ID: {p.Id} | Judul: {p.Title} | Status: {p.Status}");
            // Menampilkan hasil RELASI
            Console.WriteLine($"  -> Jumlah Developer yang ditugaskan: {p.Developers?.Count ?? 0}");
            Console.WriteLine($"  -> Jumlah Tugas (Tasks): {p.TaskItems?.Count ?? 0}");
        }
    }

    static async Task SeedDatabaseAsync(SistemManagementProyekDbContext context)
    {
        if (await context.Projects.AnyAsync())
        {
            return;
        }
        
        Console.WriteLine("Melakukan Seeding Data awal Sistem Manajemen Proyek...");

        // add developer
        var dev1 = new Developer { Name = "Lorem", Skill = "Backend .NET" };
        var dev2 = new Developer { Name = "Venrir", Skill = "Frontend React" };
        
        context.Developers.AddRange(dev1, dev2);
        await context.SaveChangesAsync();
        
        // add project dan task item
        var projectWebEComerce = new Project
        {
            Title = "Website E-Commerce",
            Status = ProjectStatus.InProgress,
            TaskItems = new List<TaskItem>
            {
                new TaskItem { Title = "Setup Database SQL", IsCompleted = true },
                new TaskItem { Title = "Integrasi Payment Gateway", IsCompleted = false }
            }
        };
        
        var projectAppMobileHR = new Project 
        { 
            Title = "Aplikasi Mobile HR", 
            Status = ProjectStatus.NotStarted,
            TaskItems = new List<TaskItem>
            {
                new TaskItem { Title = "Desain UI/UX", IsCompleted = false }
            }
        };
        
        context.Projects.AddRange(projectWebEComerce, projectAppMobileHR);
        await context.SaveChangesAsync();
        
        // add relasi many to many
        // menugaskan lorem dan venrir ke project web e-commerce
        projectWebEComerce.Developers = new List<Developer> { dev1, dev2 };
        
        // mengugaskan venrir saja ke project app mobile hr
        projectAppMobileHR.Developers = new List<Developer> { dev2 };
        
        await context.SaveChangesAsync();
        
        Console.WriteLine("Seeding Data Selesai....\n");
    }
}