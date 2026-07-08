using Microsoft.EntityFrameworkCore;
using SistemManagementProjectAPI.Data;
using SistemManagementProjectAPI.Models;
using SistemManagementProjectAPI.Models.Enums;

namespace SistemManagementProjectAPI.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllActiveAsync()
    {
        var projects = await _context.Projects
            .Where((p) => p.IsActive == true)
            .Include((p) => p.Developers)
            .Include((p) => p.TaskItems)
            .ToListAsync();
        
        return projects;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        var project = await _context.Projects
            .Include((p) => p.Developers)
            .Include((p) => p.TaskItems)
            .FirstOrDefaultAsync((p) => p.Id == id);
        
        return project;
    }

    public async Task<bool> ExistsByTitleAsync(string title)
    {
        var project = await _context.Projects.AnyAsync((p) => p.Title == title);

        return project;
    }

    public async Task<List<Project>> GetFilteredAsync(string? searchTitle, ProjectStatus? status)
    {
        var query = _context.Projects
            .Where((p) => p.IsActive)
            .Include((p) => p.Developers)
            .Include((p) => p.TaskItems)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTitle))
        {
            query = query.Where((p) => p.Title.ToLower().Contains(searchTitle.ToLower()));
        }

        if (status.HasValue)
        {
            query = query.Where((p) => p.Status == status);
        }
        
        var result = await query.ToListAsync();
        
        return result;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        
        return project;
    }

    public async Task<bool> DeactivateAsync(Guid id)
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

    public async Task<bool> AssignDeveloperAsync(Guid projectId, Guid developerId)
    {
        var project = await _context.Projects
            .Include((p) => p.Developers)
            .FirstOrDefaultAsync((p) => p.Id == projectId);

        var developer = await _context.Developers
            .FindAsync(developerId);

        if (project == null || developer == null)
        {
            return false;
        }

        if (project.Developers.Any((d) => d.Id == developerId))
        {
            return false;
        }
        
        project.Developers.Add(developer);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnassignDeveloperAsync(Guid projectId, Guid developerId)
    {
        var project = await _context.Projects
            .Include((p) => p.Developers)
            .FirstOrDefaultAsync((p) => p.Id == projectId);

        if (project == null)
        {
            return false;
        }
        
        var developer = project.Developers
            .FirstOrDefault((d) => d.Id == developerId);

        if (developer == null)
        {
            return false;
        }
        
        project.Developers.Remove(developer);
        await _context.SaveChangesAsync();

        return true;
    }
}