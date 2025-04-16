using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Backend.Endpoints;
using FamilyBudgetTracker.Backend.ExceptionHandlers;
using FamilyBudgetTracker.Backend.Extensions;
using FamilyBudgetTracker.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("ApplicationDb"));
});

builder.Services.AddIdentity<User, IdentityRole>(options => { options.User.RequireUniqueEmail = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddSignInManager();

// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateIssuerSigningKey = true,
//             ValidateLifetime = true,
//             // ValidIssuer = configuration["Jwt:Issuer"],
//             // ValidAudience = configuration["Jwt:Audience"],
//             // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
//             ClockSkew = TimeSpan.FromSeconds(5),
//         };
//     });
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy(AppConstants.PolicyNames.AdminRolePolicyName,
//         p => { p.RequireClaim(AppConstants.ClaimTypes.ClaimRoleType, AppConstants.ClaimNames.AdminRoleClaimName); });
//
//     options.AddPolicy(AppConstants.PolicyNames.UserRolePolicyName,
//         p => { p.RequireClaim(AppConstants.ClaimTypes.ClaimRoleType, AppConstants.ClaimNames.UserRoleClaimName); });
// });


//For custom exception handlers -> https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
//builder.Services.AddExceptionHandler<>();

//Global exception handler should be last
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCategoryServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("FamilyBudgetTracker.Api")
            .WithLayout(ScalarLayout.Modern)
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        // .WithApiKeyAuthentication();
    });
}

app.MapPersonalEndpoints();

app.UseHttpsRedirection();

app.Run();