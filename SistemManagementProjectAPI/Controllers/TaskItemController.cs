using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemManagementProjectAPI.DTOs.TaskItem;
using SistemManagementProjectAPI.Services;

namespace SistemManagementProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly IValidator<CreateTaskItemDto> _createValidator;
    private readonly IValidator<UpdateTaskItemDto> _updateValidator;

    public TaskItemController(
        ITaskItemService taskItemService,
        IValidator<CreateTaskItemDto> createValidator,
        IValidator<UpdateTaskItemDto> updateValidator)
    {
        _taskItemService = taskItemService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _taskItemService.GetByIdAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskItemDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _taskItemService.CreateAsync(createDto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskItemDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            return BadRequest(new { IsSuccess = false, Message = "Validation failed", Errors = errors });
        }

        var result = await _taskItemService.UpdateAsync(id, updateDto);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskItemService.DeleteAsync(id);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        
        return NoContent();
    }
}