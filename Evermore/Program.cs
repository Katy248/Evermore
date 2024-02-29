using Evermore.Data;
using Evermore.Modules;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthenticationCore();
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

builder.Services
    .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
    .AddHttpContextAccessor()
    .AddScoped<HttpContextAccessor>()
    .AddScoped<ProtectedSessionStorage>();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data source=db.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();

app.UseRouting();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
