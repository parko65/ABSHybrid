using ABSHybrid.Extensions;
using Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using NLog;

namespace ABSHybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents();

            builder.SetupConfiguration();

            builder.Services.ConfigureLoggerService();

#if DEBUG
            var env = "Development";
#else
        var env = "Production";
#endif

            builder.Services.AddSingleton<IHostEnvironment>(sp =>
            new HostingEnvironment
            {
                EnvironmentName = env, // Or get from config
                ApplicationName = AppDomain.CurrentDomain.FriendlyName
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerManager>();

            logger.LogInfo("Application started successfully.");

            return app;
        }
    }
}
