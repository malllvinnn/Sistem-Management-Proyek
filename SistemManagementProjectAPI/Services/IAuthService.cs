using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Auth;

namespace SistemManagementProjectAPI.Services;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId);
}