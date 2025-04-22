using FamilyBudgetTracker.Backend.Configuration;
using FamilyBudgetTracker.Backend.Data;
using FamilyBudgetTracker.Backend.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
//Open API documentation
builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});


//Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("ApplicationDb"));
});

//Cors
const string corsPolicy = "AllowedOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy => { policy.WithOrigins(configuration.GetSection("FrontEndUrl").Value!); });
});

//Auth
builder.Services.AddApplicationAuthenticationServices(configuration);

//Cache
builder.Services.AddApplicationCachingServices(configuration);

//Exception handlers
builder.Services.AddApplicationExceptionHandlers();

//Services
builder.Services.AddApplicationServices();

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

app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

//Map endpoints
app.MapApplicationEndpoints();

app.Run();