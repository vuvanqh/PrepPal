using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrepPal_.Backend;
using PrepPal_.Backend.Hubs;
using Serilog;


namespace PrepPal_.UI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureServices(builder.Configuration);

        //serilog
        builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration) //give serilog permission to read the config from appsettings.json
                               .ReadFrom.Services(services); //read the services & make them available to the serilog
        });

        //builder.Services.AddHttpLogging(options =>
        //{
        //    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
        //});

        var app = builder.Build();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.MapHub<ChatHub>("/chat");
        app.MapHub<NotificationHub>("/notification");

        app.Run();
    }
}
