using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemManagementProjectAPI.DTOs.Developer;
using SistemManagementProjectAPI.Services;

namespace SistemManagementProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DevelopersController : ControllerBase
{
    private readonly IDeveloperService _developerService;
    private readonly IValidator<CreateDeveloperDto> _createValidator;
    private readonly IValidator<UpdateDeveloperDto> _updateValidator;
    
    public DevelopersController(
        IDeveloperService developerService,
        IValidator<CreateDeveloperDto> createValidator,
        IValidator<UpdateDeveloperDto> updateValidator)
    {
        _developerService = developerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _developerService.GetAllAsync();

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _developerService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDeveloperDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _developerService.CreateAsync(createDto);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeveloperDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _developerService.UpdateAsync(id, updateDto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _developerService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return NoContent();
    }
}