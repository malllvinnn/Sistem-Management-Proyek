using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SistemManagementProjectAPI.Common;
using SistemManagementProjectAPI.DTOs.Auth;
using SistemManagementProjectAPI.Models;

namespace SistemManagementProjectAPI.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser != null)
        {
            var responseFailure = ServiceResult<AuthResponseDto>.Failure("Email already exists");

            return responseFailure;
        }

        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            CreatedAt = DateTime.UtcNow
        };
        
        // Password Auto Hashing PBKDF2 dengan HMAC-SHA256
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select((e) => e.Description).ToList();
            var responseFailure = ServiceResult<AuthResponseDto>.Failure("Register failed", errors);

            return responseFailure;
        }
        
        await _userManager.AddToRoleAsync(user, "Admin");
        
        var authResponse = await GenerateJwtTokenAsync(user);
        var responseSuccess = ServiceResult<AuthResponseDto>.Success(authResponse, "Register success");

        return responseSuccess;
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            var responseFailure = ServiceResult<AuthResponseDto>.Failure("Email or Password is invalid");

            return responseFailure;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            var responseFailure = ServiceResult<AuthResponseDto>.Failure("Email or Password is invalid");

            return responseFailure;
        }
        
        var authResponse = await GenerateJwtTokenAsync(user);
        var responseSuccess = ServiceResult<AuthResponseDto>.Success(authResponse, "Login success");

        return responseSuccess;
    }

    public async Task<ServiceResult<UserDto>> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            var responseFailure = ServiceResult<UserDto>.Failure("User not found");

            return responseFailure;
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            Roles = roles.ToList()
        };
        
        var responseSuccess = ServiceResult<UserDto>.Success(userDto);

        return responseSuccess;
    }

    private async Task<AuthResponseDto> GenerateJwtTokenAsync(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var key = Encoding.ASCII.GetBytes(secret);
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim("FirstName", user.FirstName ?? ""),
            new Claim("LastName", user.LastName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var expirationDays = int.Parse(jwtSettings["ExpirationDays"] ?? "7");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(expirationDays),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CreatedAt = user.CreatedAt,
            Roles = roles.ToList()
        };

        var authResponse = new AuthResponseDto
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = tokenDescriptor.Expires.Value,
            User = userDto
        };

        return authResponse;
    }
}