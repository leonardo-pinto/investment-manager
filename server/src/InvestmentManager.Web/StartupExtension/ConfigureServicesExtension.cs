﻿using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.Infrastructure.AppDbContext;
using InvestmentManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InvestmentManager
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddHttpClient();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext"));
            });

            services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddUserManager<UserManager<IdentityUser>>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtIssuer"], // move to appsettings.json
                        ValidAudience = configuration["JwtAudience"], // move to appsettings.jsop
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSecret"])) // move to secret file
                    };
                });

            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IBrApiRepository, BrApiRepository>();
            services.AddScoped<IBrApiService, BrApiService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStockPositionService, StockPositionService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IFinnhubService, FinnhubService>();
            services.AddScoped<IStockPositionRepository, StockPositionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();

            return services;
        }
    }
}
