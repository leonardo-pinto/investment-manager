﻿using InvestmentManager.ApplicationCore.Interfaces;
using InvestmentManager.ApplicationCore.Mapper;
using InvestmentManager.ApplicationCore.Services;
using InvestmentManager.Infrastructure.AppDbContext;
using InvestmentManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace InvestmentManager
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddHttpClient();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

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

            services.AddMemoryCache();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IStockQuoteService, StockQuoteService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStockPositionService, StockPositionService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IStockPositionRepository, StockPositionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IStockQuoteService, StockQuoteService>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            return services;
        }
    }
}
