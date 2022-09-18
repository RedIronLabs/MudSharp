using MudSharp.Server.Core;
using MudSharp.Server.Providers;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using System;
using System.Threading;

namespace MudSharp.Server
{
    class Program
    {
        private static IKernel _kernel;
        private static TcpServer _server;
        private static Game _game;

        static void Main(string[] args)
        {
            ConfigureNinject();

            Console.WriteLine("Starting server");
            _server = new TcpServer(_kernel.Get<IConfigProvider>(), _kernel.Get<ILoggingProvider>());
            _server.StartServer();
            _server.Listen();

            Console.WriteLine("Starting game");
            _game = new Game(_kernel.Get<IConfigProvider>(), _kernel.Get<ILoggingProvider>());
            _game.Run();

            // Since Game.Run() is blocking, we should only get here if the server is shutting down.
            _server.Shutdown();
        }

        #region DI Configuration
        /// <summary>
        /// Configures the Ninject container.
        /// </summary>
        private static void ConfigureNinject()
        {
            _kernel = new StandardKernel();

            // Core
            _kernel.Bind<IConfigProvider>().To<GlobalConfigProvider>();
            _kernel.Bind<IAuthProvider>().To<LocalAuthProvider>();
            _kernel.Bind<ILoggingProvider>().To<LocalLoggingProvider>();
        }

        #endregion
    }
}
