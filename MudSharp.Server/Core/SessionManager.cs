using MudSharp.Data.Models.World.Actors;
using MudSharp.Server.Providers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MudSharp.Server.Core
{
    /// <summary>
    /// Singleton for managing session states throughout the game.
    /// </summary>
    internal sealed class SessionManager
    {
        /// <summary>
        /// The instance of the <see cref="SessionManager"/>.
        /// </summary>
        private static SessionManager _instance = null;

        /// <summary>
        /// Lock object for thread safety.
        /// </summary>
        private static readonly object _lockObject = new object();

        /// <summary>
        /// The injected logging provider to use.
        /// </summary>
        private ILoggingProvider _loggingProvider;

        /// <summary>
        /// Constructor.
        /// </summary>
        private SessionManager()
        {
            Descriptors = new HashSet<Descriptor>();
            CurrentPlayers = new KeyValuePair<string, Player>();
            
        }


        /// <summary>
        /// Gets the current <see cref="SessionManager"/> instance.
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new SessionManager();
                    }

                    return _instance;
                }
            }
        }

        #region Properties

        public HashSet<Descriptor> Descriptors { get; set; }
        public KeyValuePair<string, Player> CurrentPlayers { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new <see cref="Descriptor"/> from the <see cref="TcpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> to create the descriptor from.</param>
        public async Task NewDescriptorAsync(TcpClient client)
        {
            var newDescriptor = new Descriptor(client);
            Descriptors.Add(newDescriptor);

            await newDescriptor.SendAsync("Username (new for new account): ");
        }

        /// <summary>
        /// Removes the descriptor from the global descriptor list and closes the underlying socket.
        /// </summary>
        /// <param name="descriptor">The descriptor to disconnect.</param>
        public void Close(Descriptor descriptor)
        {
            var endpoint = descriptor.Client.Client.RemoteEndPoint;

            try
            {
                descriptor.Client.Close();
                descriptor.Client.Dispose();
                Descriptors.Remove(descriptor);

                _loggingProvider.LogMessage($"Connection from {endpoint.ToString()} closed");
            }
            catch (Exception e)
            {
                _loggingProvider.LogMessage($"Could not close connection from {endpoint.ToString()}");
                _loggingProvider.LogMessage($"Exception at SessionManager.Close(): {e.Message}");
            }
            
        }

        /// <summary>
        /// Sends a message to all connected descriptors.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendToEveryone(string message)
        {
            foreach (var desc in Descriptors)
            {
                if (desc.IsConnected)
                    desc.Send(message);
            }
        }

        #endregion
    }
}
