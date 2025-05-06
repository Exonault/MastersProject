using FamilyBudgetTracker.Backend.API.Endpoints;
using FamilyBudgetTracker.Backend.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

//Open API documentation
builder.Services.AddOpenApiServices();

//Database
builder.Services.AddDatabase(configuration);

//Cors
// builder.Services.AddCorsServices(configuration);

//Auth
builder.Services.AddApplicationAuthenticationServices(configuration);
builder.Services.AddApplicationAuthorizationServices();

//Cache
builder.Services.AddApplicationCachingServices(configuration);

//Send email service
builder.Services.AddEmailServices(configuration);

//Hangfire
// builder.Services.AddHangfireServices(configuration);

//Exception handlers
builder.Services.AddApplicationExceptionHandlers();

//Services
builder.Services.AddEntityServices();

//Validation
builder.Services.AddFluentValidationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureScalar();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

// app.UseCors(ApplicationConstants.Cors.CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

//Map endpoints
app.MapApplicationEndpoints();

app.Run();