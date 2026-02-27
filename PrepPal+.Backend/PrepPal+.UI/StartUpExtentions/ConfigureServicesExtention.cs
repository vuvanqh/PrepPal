using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PrepPal_.Core;
using PrepPal_.Core.Application.Services;
using PrepPal_.Core.ClientContracts;
using PrepPal_.Core.Domain;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.Domain.RepositoryContracts;
using PrepPal_.Core.ServiceContracts;
using PrepPal_.Core.Services;
using PrepPal_.Infrastructure;
using PrepPal_.Infrastructure.Clients;
using PrepPal_.Infrastructure.DbContexts;
using PrepPal_.Infrastructure.Repositories;
using PrepPal_.Infrastructure.Repositories.Social;
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

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
                Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
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

        //signalR
        services.AddSignalR();


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
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IRecipeInteractionService, RecipeInteractionService>();
        services.AddScoped<IConnectionService, ConnectionService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddTransient<CartInvitationPolicy>();


        services.AddSingleton<IUserIdProvider, NameIdentifierUserIdProvider>();
        //Dispatchers
        services.AddScoped<InteractionDispatcher>();
        services.AddScoped<ConnectionCommandDispatcher>();

        //handler services
        services.AddScoped<IInteractionHandler, LikeInteractionHandler>();
        //services.AddScoped<IInteractionHandler, CartInteractionHandler>();

        //connection commands
        services.AddScoped<IConnectionCommand, AcceptConnectionCommand>();
        services.AddScoped<IConnectionCommand, RemoveConnectionCommand>();
        services.AddScoped<IConnectionCommand, RejectConnectionCommand>();
        services.AddScoped<IConnectionCommand, CancelConnectionCommand>();

        //http services
        services.AddHttpClient<IMealDbClient, MealDbClient>(client => {
            client.BaseAddress = new Uri(config["RecipeURL:Default"]!);
        });

        //repo
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeCategoryRepository, RecipeCategoryRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IIngredientRepository, IngredientRepository>();
        services.AddScoped<IRecipeInteractionRepository, RecipeInteractionRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IConnectionRepository, ConnectionRepository>();
        services.AddScoped<ICartInvitationRepository, CartInvitationRepository>();

        //dbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("Default"), sqlOptions =>
            {
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            
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

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/chat") ||
                         path.StartsWithSegments("/notification")))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });


        //authorization
        services.AddAuthorization();

        services.AddHostedService<CategoryBootstrapService>();
    }
}
