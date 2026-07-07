using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Repositories;

public interface IDeveloperRepository
{
    Task<List<Developer>> GetAllAsync();
    Task<Developer?> GetByIdAsync(Guid id);
    Task<Developer> CreateAsync(Developer developer);
    Task<Developer> UpdateAsync(Developer developer);
    Task<bool> DeleteAsync(Guid id);
}