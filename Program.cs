using HolookorBackend.Core.Application.Authentication;
using HolookorBackend.Core.Application.Exceptions;
using HolookorBackend.Core.Application.Interfaces.Repositories;
using HolookorBackend.Core.Application.Interfaces.Services;
using HolookorBackend.Core.Application.Responses;
using HolookorBackend.Core.Application.Services;
using HolookorBackend.Infrastructure.Context;
using HolookorBackend.Infrastructure.Email;
using HolookorBackend.Infrastructure.Persistence;
using HolookorBackend.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<HolookorSystem>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("HolookorBackendDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("HolookorBackendDB"))
    )
);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");
builder.Services.AddHealthChecks();

builder.Services.AddScoped<IJWTAuthenticationManager, JWTAuthenticationManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IParentService, ParentService>();
builder.Services.AddScoped<ITutorService, TutorService>();

builder.Services.AddScoped<IUserRepo, UserRepositories>();
builder.Services.AddScoped<IStudentRepo, StudentRepositories>();
builder.Services.AddScoped<IParentRepo, ParentRepositories>();
builder.Services.AddScoped<ITutorRepo, TutorRepositories>();
builder.Services.AddScoped<IUserProfileRepo, UserProfileRepositories>();
builder.Services.AddScoped(typeof(IBaseRepositoriesResponse<>), typeof(BaseRespositories<>));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddHostedService<VerificationCleanupWorker>();
builder.Services.AddScoped<IEmailVerificationRepo, EmailVerificationRepo>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["JWTSettings:SecretKey"])
            ),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["JWTSettings:Issuer"],
            ValidAudience = builder.Configuration["JWTSettings:Audience"]
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Holookor API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();




    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Holookor API v1");
        c.RoutePrefix = string.Empty;
    });

app.MapGet("/", () => Results.Ok("API is running"));
app.UseHealthChecks("/health");

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
