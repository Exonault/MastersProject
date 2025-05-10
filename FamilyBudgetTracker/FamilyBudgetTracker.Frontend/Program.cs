using FamilyBudgetTracker.Frontend.Authentication;
using FamilyBudgetTracker.Frontend.Components;
using FamilyBudgetTracker.Frontend.Constants;
using FamilyBudgetTracker.Frontend.Interfaces;
using FamilyBudgetTracker.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddServerSideBlazor()
    .AddHubOptions(options => { options.MaximumReceiveMessageSize = null; });

builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationConstants.PolicyNames.AdminRolePolicyName, p =>
    {
        p.RequireClaim(AuthorizationConstants.ClaimTypes.ClaimRoleType,
            AuthorizationConstants.ClaimValues.AdminRoleClaimName);

        p.RequireClaim(AuthorizationConstants.ClaimTypes.ClaimUserIdType);
    })
    .AddPolicy(AuthorizationConstants.PolicyNames.UserRolePolicyName, p =>
    {
        p.RequireClaim(AuthorizationConstants.ClaimTypes.ClaimRoleType,
            AuthorizationConstants.ClaimValues.UserRoleClaimName);

        p.RequireClaim(AuthorizationConstants.ClaimTypes.ClaimUserIdType);
    });

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();