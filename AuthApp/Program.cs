using AuthApp.Data;
using IdentityApiExample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
string FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "DevCorsPolicy", policy =>
    {
        policy.WithOrigins(FrontendUrl) // Allow the frontend's origin
              .AllowAnyMethod()         // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader()         // Allow all headers
              .AllowCredentials();      // Allow cookies or credentials
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Demo",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter a Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { } // Correct use of brackets
        }
    });
});



// 1.add dbcontext
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// 2.add authentication
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailConfirmed", policy =>
        policy.RequireClaim("email_verified", "true"));
});
// 3.add identity
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();

var app = builder.Build();


//4. map identity api
app.MapIdentityApi<IdentityUser>();
// Ensure CORS middleware is added before other middleware that handles requests
app.UseCors("DevCorsPolicy");

// Ensure authentication and authorization middleware are added in the correct order
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();