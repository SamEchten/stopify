using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Stopify;
using Stopify.Middleware.Exception;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stopify.Attribute.Auth;
using Stopify.Hubs;
using Stopify.Services.Streaming;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddControllers(options => {
    options.Filters.Add<ExceptionFilter>();
    options.Filters.Add<AuthorizeUserActionFilter>();
    options.Filters.Add<AuthorizeArtistActionFilter>();
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {

                var token = context.Request.Cookies["Stopify-AccessToken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                context.Token = context.Request.Cookies["Stopify-AccessToken"];

                return Task.CompletedTask;
            }
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebRTCHub>("/webrtchub");
    endpoints.MapHub<SessionHub>("/sessionhub");
});

app.Run();