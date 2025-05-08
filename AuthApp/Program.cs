using AuthApp.Data;
using IdentityApiExample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Add configuration values
string frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

// 2. Register services
builder.Services.AddControllers();

// Entity Framework + Identity stores
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>();

// Email sender for confirmation emails
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailConfirmed", policy =>
        policy.RequireClaim("email_verified", "true"));
});

// Swagger/OpenAPI (with JWT Bearer support)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Demo",
        Version = "v1"
    });

    // JWT bearer token support in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter ‘Bearer’ [space] and then your valid JWT token.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS for your frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// 3. HTTP request pipeline

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Demo v1");
    });
}

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// CORS must come early in the pipeline
app.UseCors("DevCorsPolicy");

// Authentication + Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Identity “minimal API” endpoints (e.g. /register, /login, /confirm-email)
app.MapIdentityApi<IdentityUser>();

// Map your controllers
app.MapControllers();

app.Run();
