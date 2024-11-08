using EnglishPremierLeague2024.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interfaces;
using Services.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
     }); 

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Description =
            @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345example'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
}
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = configuration["JwtSettings:Issuer"],
        ValidAudience = configuration["JwtSettings:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? ""))
    };
});


builder.Services.AddScoped<IPremierLeagueAccountService, PremierLeagueAccountService>();
builder.Services.AddScoped<IFootballPlayerService, FootballPlayerService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPremierLeagueAccountRepository, PremierLeagueAccountRepository>();
builder.Services.AddScoped<IFootballPlayerRepository, FootballPlayerRepository>();
builder.Services.AddScoped<IFootballClubRepository, FootballClubRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<EnglishPremierLeague2024DbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("1"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
