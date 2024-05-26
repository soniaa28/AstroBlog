using AstroBlog.Data;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AstroBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AstroBlogDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AstroBlogConnectionString")));
            builder.Services.AddDbContext<TheAuthDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AstroBlogAuthDbConnectionString")));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<TheAuthDbContext>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

            });
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
            builder.Services.AddScoped<IBlogPostLikesRepository, BlogPostLikesRepository>();
            builder.Services.AddScoped<IBlogPostCommentRepository, BlogPostCommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
