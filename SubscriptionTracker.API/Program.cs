using SubscriptionTracker.Infrastructure.Repositories;
using SubscriptionTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Infrastructure.Data;
using SubscriptionTracker.Application.Interfaces;
using CategoryTracker.Application.Interfaces;
using SubscriptionTracker.Application.Services;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();

builder.Services.AddDbContext<SubscriptionAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
