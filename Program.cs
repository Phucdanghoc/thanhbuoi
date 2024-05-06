using Microsoft.EntityFrameworkCore;
using ThanhBuoi.Models;
using ThanhBuoi.Data;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    private static async Task Main(string[] args)
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
                                               options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
        builder.Services.AddRazorPages();
        builder.Services.AddOptions();

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
        using(var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "ADMIN", "SALER", "USER" };
            foreach (var role in roles )
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    
                }
            }
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TaiKhoan>>();
            string email = "admin@admin.com";
            string password = "Admin123@";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new TaiKhoan
                {
                    Email = email,
                    Ten = "Admin",
                    UserName = email,
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user, password);
                await  userManager.AddToRoleAsync(user, "ADMIN");
            }
        }
        app.Run();
    }
}
