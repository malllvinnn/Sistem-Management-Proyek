using Microsoft.EntityFrameworkCore;
using SistemManagementProjectAPI.Data;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Repositories;

public class DeveloperRepository : IDeveloperRepository
{
    private readonly ApplicationDbContext _context;

    public DeveloperRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Developer>> GetAllAsync()
    {
        var developers = await _context.Developers
            .Include((d) => d.Projects)
            .ToListAsync();
        
        return developers;
    }

    public async Task<Developer?> GetByIdAsync(Guid id)
    {
        var developer = await _context.Developers
            .Include((d) => d.Projects)
            .FirstOrDefaultAsync(d => d.Id == id);

        return developer;
    }

    public async Task<Developer> CreateAsync(Developer developer)
    {
        _context.Developers.Add(developer);
        await _context.SaveChangesAsync();

        return developer;
    }

    public async Task<Developer> UpdateAsync(Developer developer)
    {
        _context.Developers.Update(developer);
        await _context.SaveChangesAsync();

        return developer;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var developer = await _context.Developers.FindAsync(id);

        if (developer == null)
        {
            return false;
        }
        
        _context.Developers.Remove(developer);
        await _context.SaveChangesAsync();

        return true;
    }
}