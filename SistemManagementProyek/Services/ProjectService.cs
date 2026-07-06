using Microsoft.EntityFrameworkCore;
using SistemManagementProyek.Data;
using SistemManagementProyek.Enums;
using SistemManagementProyek.Models;

namespace SistemManagementProyek.Services;

public class ProjectService
{
    private readonly SistemManagementProyekDbContext _context;

    public ProjectService(SistemManagementProyekDbContext context)
    {
        _context = context;
    }
    
    // CREATE: Add new project
    public async Task<Project> CreateProjectAsync(Project project)
    {
        var existingProject = await _context.Projects
            .FirstOrDefaultAsync((p) => p.Title == project.Title);

        if (existingProject != null)
        {
            throw new InvalidOperationException($"Project with title {project.Title} already exists.");
        }
        
        _context.Projects.Add(project);
        
        await _context.SaveChangesAsync();

        return project;
    }
    
    // READ: All
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        var projects = await _context.Projects
            .Where((p) => p.IsActive == true)
            .Include((p) => p.TaskItems)
            .Include((p) => p.Developers)
            .ToListAsync();

        return projects;
    }
    
    // READ: by ID
    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        var project = await _context.Projects
            .Include((p) => p.TaskItems)
            .Include((p) => p.Developers)
            .FirstOrDefaultAsync((p) => p.Id == id);

        return project;
    }
    
    // UPDATE
    public async Task<Project?> UpdateProjectAsync(int id, string updateTitle, ProjectStatus updateStatus)
    {
        var existingProject = await _context.Projects.FindAsync(id);

        if (existingProject == null)
        {
            return null;
        }
        
        existingProject.Title = updateTitle;
        existingProject.Status = updateStatus;

        await _context.SaveChangesAsync();

        return existingProject;
    }
    
    // DELETE (soft delete)
    public async Task<bool> DeactivateProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return false;
        }
        
        project.IsActive = false;
        
        await _context.SaveChangesAsync();

        return true;
    }
}