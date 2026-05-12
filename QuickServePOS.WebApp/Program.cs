using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuickServePOS.Models.ValidationModels.MVCSideValidation.AdminVmValidation;
using QuickServePOS.WebApp.AutoMapper;
using QuickServePOS.WebApp.AutoMapper.MVCSideMapper.MenuMVCMapper;
using QuickServePOS.WebApp.Filter;
using QuickServePOS.WebApp.HttpHelper;
using QuickServePOS.WebApp.Services;

namespace QuickServePOS.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<RefreshTokenFilter>();
            });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateStaffViewModelValidation>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7290/api/"); // your API URL
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Authentication/Login";
                    options.AccessDeniedPath = "/Authentication/Login";
                    options.ExpireTimeSpan =TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddScoped<RefreshTokenFilter>();
            builder.Services.AddScoped<IApiHelper, ApiHelper>();
            builder.Services.AddScoped<ITokenWebService, TokenWebService>();

            builder.Services.AddAutoMapper(typeof(CategoryViewModelProfile));


            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.Name= "QuickServePOS.Session";

                options.Cookie.HttpOnly = true;

                options.Cookie.IsEssential = true;
            });

           
            builder.Services.AddAuthorization();

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

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
