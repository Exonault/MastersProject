using Blazored.SessionStorage;
using BooksAPI.FE.Authentication;
using BooksAPI.FE.Components;
using BooksAPI.FE.Constants;
using BooksAPI.FE.Extensions;
using BooksAPI.FE.Interfaces;
using BooksAPI.FE.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options => { options.MaximumReceiveMessageSize = null; });

builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();

builder.Services.AddApplicationServices();

builder.Services.AddAuthorizationServices();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredSessionStorage();


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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();