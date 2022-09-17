using MudSharp.Server.Providers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MudSharp.Server.Core
{
    /// <summary>
    /// TCP server.
    /// </summary>
    internal sealed class TcpServer : IServer
    {
        private readonly IConfigProvider _configProvider;
        private readonly ILoggingProvider _loggingProvider;
        private bool _accept = false;
        private TcpListener _listener;
        private readonly CancellationTokenSource _tokenSource;

        public TcpServer(IConfigProvider configProvider, ILoggingProvider loggingProvider)
        {
            _configProvider = configProvider;
            _loggingProvider = loggingProvider;

            _tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Configures the TCP server and starts the listener.
        /// </summary>
        public void StartServer()
        {
            try
            {
                IPAddress address = IPAddress.Parse(_configProvider.Core.ListenAddress);
                _listener = new TcpListener(address, _configProvider.Core.ListenPort);

                _listener.Start();
                _accept = true;

                _loggingProvider.LogMessage($"Listening for connections on port {_configProvider.Core.ListenPort}");
            }
            catch (Exception e)
            {
                _loggingProvider.LogMessage($"Caught exception at TcpClient.StartServer(): {e.Message}");
                _accept = false;
            }
        }

        /// <summary>
        /// Listens on the open TCP socket for new connections.
        /// </summary>
        public void Listen()
        {
            Task.Run(async () =>
            {
                if (_listener != null && _accept)
                {
                    while (true)
                    {
                        if (_tokenSource.Token.IsCancellationRequested)
                        {
                            _loggingProvider.LogMessage("Stopping TCP listener");
                            _accept = false;
                            _listener.Stop();
                            break;
                        }

                        var clientTask = _listener.AcceptTcpClientAsync();

                        if (clientTask.Result != null)
                        {
                            var client = clientTask.Result;

                            _loggingProvider.LogMessage($"New connection from {client.Client.RemoteEndPoint.ToString()}");

                            await SessionManager.Instance.NewDescriptorAsync(client);

                        }
                    }
                }
            }, _tokenSource.Token);
        }

        /// <summary>
        /// Starts shutdown of the TCP server.
        /// </summary>
        public void Shutdown()
        {
            _loggingProvider.LogMessage("SHUTDOWN: Requesting cancellation of TCP listener task");
            _tokenSource.Cancel();
        }
    }
}
