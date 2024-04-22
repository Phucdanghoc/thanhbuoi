using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;
using ThanhBuoi.Data;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DbContext"));
        });
        builder.Services.AddDefaultIdentity<TaiKhoan>(options =>
                                               options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DataContext>();
        builder.Services.AddRazorPages();
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

        app.UseRouting();

        app.UseAuthorization();
        app.MapRazorPages();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
