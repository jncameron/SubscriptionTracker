using SubscriptionTracker.Infrastructure.Repositories;
using SubscriptionTracker.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SubscriptionTracker.Infrastructure.Data;
using SubscriptionTracker.Application.Services;
using Scalar.AspNetCore;
using SubscriptionTracker.Application.Interfaces.Repositories;
using SubscriptionTracker.Application.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();

builder.Services.AddDbContext<SubscriptionAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://many-wallaby-4.clerk.accounts.dev";
        options.Audience = "http://localhost:5173"; // must match what Clerk issues
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://many-wallaby-4.clerk.accounts.dev",
            ValidateAudience = false,
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

app.UseCors("AllowViteFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
