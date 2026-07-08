using Microsoft.EntityFrameworkCore;
using SistemManagementProjectAPI.Data;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly ApplicationDbContext _context;

    public TaskItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId)
    {
        var taskItems = await _context.TaskItems
            .Where((t) => t.ProjectId == projectId)
            .ToListAsync();

        return taskItems;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        var taskItem = await _context.TaskItems
            .Include((t) => t.Project)
            .FirstOrDefaultAsync((t) => t.Id == id);

        return taskItem;
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<TaskItem> UpdateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync();

        return taskItem;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);

        if (taskItem == null)
        {
            return false;
        }
        
        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();

        return true;
    }
}