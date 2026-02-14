using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrepPal_.Core.Domain.Entities;
using PrepPal_.Core.ServiceContracts;
using PrepPal_.Core.Services;
using PrepPal_.Infrastructure.DbContexts;

namespace PrepPal_.UI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var app = builder.Build();

        //app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
