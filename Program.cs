using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockFlow.Application.Features.AppCategory.Command.CreateCommand;
using StockFlow.Application.Features.Common.AutoMapping;
using StockFlow.Application.Features.Interfaces.Repositories;
using StockFlow.Application.Features.Interfaces.UnitOfWork;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;
using StockFlow.Infrastructure.Repository;
using StockFlow.Infrastructure.SeedRole;
using StockFlow.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// REGISTER DB 
builder.Services
.AddDbContext<StockFlowContext>(
options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString(
            "DefaultConnection"));
});
//REGISTER UNIT OF WORK
builder.Services.AddScoped<IUnitOfWork, UnitOfWorks>();

// AUTOMAPPER
// =========================
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
// registring MediatR   
builder.Services.AddMediatR(
    cfg => cfg.RegisterServicesFromAssembly(
        typeof(CreateCategoryHandler).Assembly));


// REGISTER REPOSITORIES
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();

// REGISTERING IDENTITY
builder.Services
.AddIdentity<AppUser, IdentityRole<Guid>>(
    p =>
    {
        p.Password.RequireDigit = true;
        p.Password.RequireUppercase = true;
        p.Password.RequiredLength = 6;
    }
    )


.AddEntityFrameworkStores<
StockFlowContext>()
.AddDefaultTokenProviders();




var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
        .GetRequiredService<
            RoleManager<
                IdentityRole<Guid>>>();

    await RoleSeeder
        .SeedAsync(
            roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
