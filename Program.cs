using Final_new.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services for Razor Pages and Session
builder.Services.AddRazorPages();
builder.Services.AddSession(); // Add session support

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Enable session middleware
app.UseSession();

app.MapRazorPages();
app.Run();