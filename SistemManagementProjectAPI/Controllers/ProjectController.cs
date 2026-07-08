using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemManagementProjectAPI.DTOs.Project;
using SistemManagementProjectAPI.Models.Enums;
using SistemManagementProjectAPI.Services;

namespace SistemManagementProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;
    
    public ProjectController(
        IProjectService projectService,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator)
    {
        _projectService = projectService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? title, [FromQuery] ProjectStatus? status)
    {
        if (!string.IsNullOrWhiteSpace(title) || status.HasValue)
        {
            var filtered = await _projectService.GetFilteredAsync(title, status);
            
            return Ok(filtered);
        }

        var result = await _projectService.GetAllAsync();
        
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _projectService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _projectService.CreateAsync(createDto);

        if (!result.IsSuccess)
        {
            return Conflict(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _projectService.UpdateAsync(id, updateDto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await _projectService.DeactivateAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return NoContent();
    }
    
    [HttpPost("{id}/developers/{developerId}")]
    public async Task<IActionResult> AssignDeveloper(Guid id, Guid developerId)
    {
        var result = await _projectService.AssignDeveloperAsync(id, developerId);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}/developers/{developerId}")]
    public async Task<IActionResult> UnassignDeveloper(Guid id, Guid developerId)
    {
        var result = await _projectService.UnassignDeveloperAsync(id, developerId);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    [HttpGet("{projectId}/tasks")]
    public async Task<IActionResult> GetTasks(Guid projectId, [FromServices] ITaskItemService taskItemService)
    {
        var result = await taskItemService.GetByProjectIdAsync(projectId);
        return Ok(result);
    }
}