using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Developer;

namespace SistemManagementProjectAPI.Services;

public interface IDeveloperService
{
    Task<ServiceResult<List<DeveloperDto>>> GetAllAsync();
    Task<ServiceResult<DeveloperDto>> GetByIdAsync(Guid id);
    Task<ServiceResult<DeveloperDto>> CreateAsync(CreateDeveloperDto createDto);
    Task<ServiceResult<DeveloperDto>> UpdateAsync(Guid id, UpdateDeveloperDto updateDto);
    Task<ServiceResult<object>> DeleteAsync(Guid id);
}