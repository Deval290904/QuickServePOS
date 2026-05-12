using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickServePOS.DbContextData.Data;
using QuickServePOS.Models.Configurations;
using QuickServePOS.Models.Entities;
using QuickServePOS.Models.ValidationModels;
using QuickServePOS.Repositories.IUnitofWork;
using QuickServePOS.Repositories.UnitofWork;
using QuickServePOS.Services.IService.Admin;
using QuickServePOS.Services.IService.Auth;
using QuickServePOS.Services.IService.Common;
using QuickServePOS.Services.IService.Menu;
using QuickServePOS.Services.IService.Table;
using QuickServePOS.Services.Service;
using QuickServePOS.Services.Service.Auth;
using QuickServePOS.Services.Service.Common;
using QuickServePOS.Services.Service.Menu;
using QuickServePOS.Services.Service.Table;
using QuickServePOS.WebAPI.Filter;
using QuickServePOS.WebAPI.Seed;
using QuickServePOS.WebApp.AutoMapper.Menu;
using System.Text;


namespace QuickServePOS.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================================================
            // JWT SETTINGS
            // =========================================================

            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("JwtSettings"));

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            var jwtSettings = builder.Configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            // =========================================================
            // DATABASE
            // =========================================================

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // =========================================================
            // IDENTITY
            // =========================================================

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password Settings
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Lockout Settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                // User Settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // =========================================================
            // JWT AUTHENTICATION
            // =========================================================

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;

                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings!.Issuer,

                            ValidAudience = jwtSettings.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtSettings.Key)),

                            ClockSkew = TimeSpan.Zero
                        };
                

                    // =====================================================
                    // CUSTOM JWT MESSAGES
                    // =====================================================

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = 401;

                            context.Response.ContentType = "application/json";

                            return context.Response.WriteAsJsonAsync(new
                            {
                                Status = 401,
                                Message = "Please login first to access this resource."
                            });
                        },

                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;

                            context.Response.ContentType = "application/json";

                            return context.Response.WriteAsJsonAsync(new
                            {
                                Status = 403,
                                Message = "You are not authorized to access this resource."
                            });
                        }
                    };
                });

            // =========================================================
            // AUTHORIZATION
            // =========================================================

            builder.Services.AddAuthorization();

            // =========================================================
            // CONTROLLERS + FLUENT VALIDATION
            // =========================================================

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<RegisterValidator>();
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(
                options =>
                {
                    options.TokenLifespan = TimeSpan.FromMinutes(30);
                });

            // =========================================================
            // DEPENDENCY INJECTION
            // =========================================================

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IAdminService, AdminService>();

            builder.Services.AddScoped<IProfileService, ProfileService>();

            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IMenuItemService, MenuItemService>();

            builder.Services.AddScoped<IFloorService, FloorService>();

            builder.Services.AddScoped<ITableService, TableService>();

            builder.Services.AddScoped<TableStateMachineService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(typeof(CategoryProfile));

            // =========================================================
            // SWAGGER
            // =========================================================

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "QuickServe POS API",
                        Version = "v1"
                    });

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Enter JWT Token like this: Bearer {your token}"
                    });

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            // =========================================================
            // BUILD APP
            // =========================================================

            var app = builder.Build();

            // =========================================================
            // MIDDLEWARE
            // =========================================================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            // =========================================================
            // ROLE + ADMIN SEEDING
            // =========================================================

            using (var scope = app.Services.CreateScope())
            {
                var roleManager =
                    scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                var userManager =
                    scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

                await RoleSeeder.SeedRolesAsync(roleManager);

                await AdminSeeder.SeedAdminAsync(userManager);
            }

            // =========================================================
            // RUN APP
            // =========================================================

            app.Run();
        }
    }
}