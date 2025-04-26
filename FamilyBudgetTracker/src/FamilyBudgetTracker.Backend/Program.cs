using FamilyBudgetTracker.Backend.Extensions;

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

//Cache
// builder.Services.AddApplicationCachingServices(configuration);

//Exception handlers
builder.Services.AddApplicationExceptionHandlers();

//Services
builder.Services.AddEntityServices();

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