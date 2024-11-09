using EnglishPremierLeague2024.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Entities;
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

/*
 Packages to install:

Microsoft.AspNetCore.Authentication.JwtBearer (INSTALL AT SERVICES LAYER)
Swashbuckle.AspNetCore.Filters


scaffold command:
dotnet ef dbcontext scaffold "Server=(local); Uid=sa; Pwd=1234567890; Database=EnglishPremierLeague2024DB; TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir ./

 */

/*
 OData: Install Microsoft.AspNetCore.OData
builder.Services.AddControllers()
    .AddOData(options =>
    {
        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<FootballPlayer>("FootballPlayers");
        odataBuilder.EntitySet<Club>("Clubs");
        options.AddRouteComponents("odata", odataBuilder.GetEdmModel())
               .Select().Expand().Filter().OrderBy().Count().SetMaxTop(100);
    });


Query syntax:

1. Filtering Data ($filter)
Usage: Filters data based on specified criteria.
Example: GET /odata/FootballPlayers?$filter=Age eq 25
Operators:
eq (equal to): Age eq 25
ne (not equal to): Age ne 25
gt (greater than), ge (greater or equal): Age gt 20
lt (less than), le (less or equal): Age lt 30
and, or: Age gt 20 and Age lt 30
contains, startswith, endswith: contains(Name, 'John')

2. Selecting Fields ($select)
Usage: Limits the fields returned in the response.
Example: GET /odata/FootballPlayers?$select=Fullname,Age

3. Sorting Data ($orderby)
Usage: Sorts data in ascending (asc) or descending (desc) order.
Example: GET /odata/FootballPlayers?$orderby=Age desc

4. Limiting the Number of Results ($top, $skip)
Usage: $top limits the number of results, and $skip skips a specified number of results.
Example: GET /odata/FootballPlayers?$top=5&$skip=2 (skips 2 items, returns the next 5)

5. Expanding Related Entities ($expand)
Usage: Includes related entities in the response.
Example: GET /odata/FootballPlayers?$expand=Club (includes related Club information)


6. Counting Results ($count)
Usage: Returns the total number of items that meet the query criteria.
Example: GET /odata/FootballPlayers?$count=true
7. Combining Parameters


Multiple parameters can be combined in a single query.
Example: GET /odata/FootballPlayers?$filter=Age gt 20&$orderby=Age desc&$top=10


 */


/*
 CORS:


builder.Services.AddCors(options =>
                options.AddPolicy("AllowAll", b => b.AllowAnyHeader()
                .WithExposedHeaders("X-Total-Count", "X-Total-Pages", "X-Page", "X-Page-Size")
                .AllowAnyOrigin().AllowAnyMethod()));


app.UseCors("AllowAll");

 */


builder.Services.AddControllers()
        .AddOData(options =>
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<FootballPlayer>("FootballPlayers");
            odataBuilder.EntitySet<FootballClub>("FootballClubs");
            options.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                   .Select().Expand().Filter().OrderBy().Count().SetMaxTop(100);
        })
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
