using System.Text;
using FamilyBudgetTracker.Backend.Configuration;
using FamilyBudgetTracker.Backend.Constants;
using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Backend.Endpoints;
using FamilyBudgetTracker.Backend.Endpoints.User;
using FamilyBudgetTracker.Backend.ExceptionHandlers;
using FamilyBudgetTracker.Backend.Extensions;
using FamilyBudgetTracker.Entities.Entities;
using FamilyBudgetTracker.Entities.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//Open API documentation
// builder.Services.AddOpenApi();

builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
//Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("ApplicationDb"));
});

//Auth
builder.Services.AddIdentity<User, IdentityRole>(options => { options.User.RequireUniqueEmail = true; })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddSignInManager();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.FromSeconds(5),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApplicationConstants.PolicyNames.AdminRolePolicyName,
        p => { p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType, ApplicationConstants.ClaimNames.AdminRoleClaimName); });

    options.AddPolicy(ApplicationConstants.PolicyNames.UserRolePolicyName,
        p => { p.RequireClaim(ApplicationConstants.ClaimTypes.ClaimRoleType, ApplicationConstants.ClaimNames.UserRoleClaimName); });
});


//Exception handlers
builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<InvalidEmailExceptionHandler>();
builder.Services.AddExceptionHandler<UserAlreadyRegisteredExceptionHandler>();
builder.Services.AddExceptionHandler<UserNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<MappingExceptionHandler>();
//Global exception handler should be last
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//Services
builder.Services.AddCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("FamilyBudgetTracker.Api")
            .WithLayout(ScalarLayout.Modern)
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseExceptionHandler();

app.MapPersonalEndpoints();
app.MapUserEndpoints();

app.UseHttpsRedirection();

app.Run();