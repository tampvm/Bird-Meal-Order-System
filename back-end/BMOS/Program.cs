using BMOS.Models.Entities;
using BMOS.Models.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<BmosContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect"));
});

services.AddAuthentication().AddGoogle(googleOptions =>
{
	googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
	googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
	googleOptions.CallbackPath = "/Account/LoginWithGoogle";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
