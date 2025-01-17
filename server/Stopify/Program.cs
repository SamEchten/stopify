using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Stopify;
using Stopify.Middleware.Exception;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stopify.Attribute.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddControllers(options => {
    options.Filters.Add<ExceptionFilter>();
    options.Filters.Add<AuthorizeUserActionFilter>();
});

ServiceRegistration.RegisterServices(builder, Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(10, 4, 32))));

var key = builder.Configuration.GetSection("AppSettings:Key").Value ?? throw new Exception("No key provided in appsettings");
var issuer = builder.Configuration.GetSection("AppSettings:Issuer").Value ?? throw new Exception("No issuer provided in appsettings");
var audience = builder.Configuration.GetSection("AppSettings:Audience").Value ?? throw new Exception("No audience provided in appsettings");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();