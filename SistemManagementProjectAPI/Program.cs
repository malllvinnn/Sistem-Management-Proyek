using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemManagementProjectAPI.Data;
using SistemManagementProjectAPI.DTOs.Developer;
using SistemManagementProjectAPI.DTOs.Project;
using SistemManagementProjectAPI.DTOs.TaskItem;
using SistemManagementProjectAPI.Models;
using SistemManagementProjectAPI.Repositories;
using SistemManagementProjectAPI.Services;
using SistemManagementProjectAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>((options) =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>((options) =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Fluent Validator
builder.Services.AddScoped<IValidator<CreateProjectDto>, CreateProjectValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectDto>, UpdateProjectValidator>();
builder.Services.AddScoped<IValidator<CreateDeveloperDto>, CreateDeveloperValidator>();
builder.Services.AddScoped<IValidator<UpdateDeveloperDto>, UpdateDeveloperValidator>();
builder.Services.AddScoped<IValidator<CreateTaskItemDto>, CreateTaskItemValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskItemDto>, UpdateTaskItemValidator>();

// Add Repository Layer
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IDeveloperRepository, DeveloperRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();

// Add Service Layer
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IDeveloperService, DeveloperService>();
builder.Services.AddScoped<ITaskItemService, TaskItemService>();

// Constrollers
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
