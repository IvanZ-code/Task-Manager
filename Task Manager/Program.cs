using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Task_Manager.Data;
using Task_Manager.Interfaces;
using Task_Manager.Services;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey) ||
    jwtKey == "__FROM_USER_SECRETS__")
{
    throw new InvalidOperationException(
        "JWT key is not configured.");
}

var connectionString = builder.Configuration.GetConnectionString("PostgreSql");

if (string.IsNullOrWhiteSpace(connectionString) ||
    connectionString == "__FROM_USER_SECRETS__")
{
    throw new InvalidOperationException(
        "Database connection string is not configured.");
}

// Add services to the container.
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IHistoryService, HistoryService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",

        Type = SecuritySchemeType.Http,

        Scheme = "Bearer",

        BearerFormat = "JWT",

        In = ParameterLocation.Header,

        Description =
            "Enter JWT token in format: Bearer {token}"
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

            new string[] {}
        }
    });
});


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,


            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!
                    )
                )
        };
    });


// Authorization

builder.Services.AddAuthorization();


// CORS для React

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173");
        });
});


var app = builder.Build();


// HTTP pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.UseCors("ReactPolicy");


app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();


app.Run();

