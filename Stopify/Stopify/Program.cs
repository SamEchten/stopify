using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Stopify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ServiceRegistration.RegisterServices(builder.Services, Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21)))); // Ensure the version matches your MySQL version

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();