using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudSharp.Server.Core;
using MudSharp.Server.Providers;

var host = CreateHostBuilder(args)
                .Build();

var server = host.Services.GetService<IServer>();
var game = host.Services.GetService<IGame>();

server.StartServer();
server.Listen();

game.Run();

// Since Game.Run() is blocking, we should only get here if the server is shutting down.
server.Shutdown();

#region DI Configuration
/// <summary>
/// Configures the DI container.
/// </summary>
static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services
               .AddSingleton<IConfigProvider, GlobalConfigProvider>()
               .AddTransient<IAuthProvider, LocalAuthProvider>()
               .AddSingleton<ILoggingProvider, LocalLoggingProvider>()
               .AddSingleton<IServer, TcpServer>()
               .AddSingleton<IGame, Game>(); ;
        });

    return hostBuilder;
}

#endregion
