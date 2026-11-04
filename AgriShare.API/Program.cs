using AgriShare.Application.Common.Interfaces;
using AgriShare.Application.ServiceRequests.Commands;
using AgriShare.Application.ServiceRequests.Queries;
using AgriShare.Application.ServiceExecutions.Commands;
using AgriShare.Application.Services.Commands;
using AgriShare.Application.Services.Queries;
using AgriShare.Application.Users.Commands;
using AgriShare.Application.Users.Queries;
using AgriShare.Infrastructure.Persistence;
using AgriShare.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "The 'DefaultConnection' connection string is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.UseNetTopologySuite()));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<CreateServiceRequestCommandHandler>();
builder.Services.AddScoped<GetServiceRequestByIdQueryHandler>();
builder.Services.AddScoped<StartServiceExecutionCommandHandler>();
builder.Services.AddScoped<CompleteServiceExecutionCommandHandler>();
builder.Services.AddScoped<CreateServiceCommandHandler>();
builder.Services.AddScoped<UpdateServiceCommandHandler>();
builder.Services.AddScoped<DeactivateServiceCommandHandler>();
builder.Services.AddScoped<GetServicesQueryHandler>();
builder.Services.AddScoped<GetServiceByIdQueryHandler>();
builder.Services.AddScoped<GetNearbyServicesQueryHandler>();
builder.Services.AddScoped<CreateUserCommandHandler>();
builder.Services.AddScoped<GetUsersQueryHandler>();
builder.Services.AddScoped<GetUserByIdQueryHandler>();
builder.Services.AddScoped<UpdateUserCommandHandler>();
builder.Services.AddScoped<DeactivateUserCommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgriShare API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
