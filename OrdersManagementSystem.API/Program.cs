using Microsoft.OpenApi.Models;
using OrdersManagementSystem.Domain.Interfaces;
using OrdersManagementSystem.Infrastructure.Repositories;
using OrdersManagementSystem.Application.Services;
using OrdersManagementSystem.Application.DTOs;
using OrdersManagementSystem.Domain.Model;
using OrdersManagementSystem.Infrastructure.Middleware;
using FluentValidation.AspNetCore;
using OrdersManagementSystem.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<OrderValidator>());

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register specific validators
builder.Services.AddScoped<IOrderRepository, OrderRepository>(); // Correct registration
builder.Services.AddScoped<IGenericService<OrderDTO, Order>, OrderService>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders API", Version = "v1" });
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
