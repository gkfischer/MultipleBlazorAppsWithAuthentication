using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MultipleBlazorApps.Server.Data;
using MultipleBlazorApps.Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
app.MapWhen(ctx => ctx.Request.Host.Port == 5001 ||
                   ctx.Request.Host.Equals("firstapp.com"), first =>
{
    first.Use((ctx, nxt) =>
    {
        ctx.Request.Path = "/FirstApp" + ctx.Request.Path;
        return nxt();
    });

    first.UseBlazorFrameworkFiles();
    first.UseStaticFiles();
    first.UseStaticFiles("/FirstApp");
    first.UseRouting();

    first.UseIdentityServer();
    first.UseAuthentication();
    first.UseAuthorization();

    first.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("/FirstApp/{*path:nonfile}",
            "FirstApp/index.html");
    });
});

app.MapWhen(ctx => ctx.Request.Host.Port == 5002 ||
                   ctx.Request.Host.Equals("secondapp.com"), second =>
{
    second.Use((ctx, nxt) =>
    {
        ctx.Request.Path = "/SecondApp" + ctx.Request.Path;
        return nxt();
    });

    second.UseBlazorFrameworkFiles();
    second.UseStaticFiles();
    second.UseStaticFiles("/SecondApp");
    second.UseRouting();

    second.UseIdentityServer();
    second.UseAuthentication();
    second.UseAuthorization();

    second.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("/SecondApp/{*path:nonfile}",
            "SecondApp/index.html");
    });
});
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();