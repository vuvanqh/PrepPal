using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrepPal_.Core.Application.ServiceContracts;
using PrepPal_.Core.Application.Services;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using PrepPal_.Core.Services;
using PrepPal_.Infrastructure;
using PrepPal_.Infrastructure.Clients;
using PrepPal_.Infrastructure.DbContexts;
using PrepPal_.Infrastructure.Repositories;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace PrepPal_.Backend;

public static class ConfigureServicesExtention
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
    { 
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        //cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy
                        .WithOrigins(config.GetSection("AllowedOrigins").Get<string[]>()!)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        //controllers
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add(new ProducesAttribute("application/json")); 
            options.Filters.Add(new ConsumesAttribute("application/json"));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter()
            );
        });

        //services

        services.AddTransient<IJwtService, JwtService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        
        services.AddScoped<IRecipeService, RecipeService>();

        //http services
        services.AddHttpClient<IMealDbClient, MealDbClient>(client => {
            client.BaseAddress = new Uri(config["RecipeURL:Default"]!);
        });

        //repo
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeCategoryRepository, RecipeCategoryRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();

        //dbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("Default"));
        });

        //identity

        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


        //Jwt
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = config["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
            };
        });


        //authorization
        services.AddAuthorization();

        services.AddHostedService<CategoryBootstrapService>();
    }
}
