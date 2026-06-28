using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockFlow.Domain.Entities;
using StockFlow.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


builder.Services


// REGISTER DB FIRST
.AddDbContext<StockFlowContext>(
options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString(
            "DefaultConnection"));
});

// THEN IDENTITY
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
