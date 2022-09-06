using MudSharp.Data.Models.Accounts;
using MudSharp.Data.Models.World.Actors;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MudSharp.Server.Core
{
    /// <summary>
    /// Descriptor connection state.
    /// </summary>
    internal enum ConnectionState
    {
        Playing,
        Disconnected,
        GetUsername,
        GetPassword,
        NewUsername,
        NewPassword,
        VerifyNewPassword,
        MainMenu
    }

    /// <summary>
    /// Class describing a client descriptor.
    /// </summary>
    internal sealed class Descriptor
    {

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client"><see cref="TcpClient"/> to create the new descriptor from.</param>
        public Descriptor(TcpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));

            // We can assume that if we are creating a new descriptor that there is already a TcpClient connected.
            ConnectionTime = DateTime.UtcNow;
            State = ConnectionState.GetUsername;

            Id = Guid.NewGuid();
            IdleTics = 0;
            Account = null;
            Player = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier for the descriptor instance.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The <see cref="TcpClient"/> to be used for socket communication.
        /// </summary>
        public TcpClient Client { get; private set; }

        /// <summary>
        /// <c>true</c> if the <see cref="TcpClient"/> is connected, otherwise <c>false</c>.
        /// </summary>
        public bool IsConnected => Client.Connected;

        /// <summary>
        /// The number of tics this descriptor has been idle (no input from client.)
        /// </summary>
        public uint IdleTics { get; set; }

        /// <summary>
        /// The time that the client was first connected.
        /// </summary>
        public DateTime ConnectionTime { get; private set; }

        /// <summary>
        /// The player account associated with this descriptor.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// The player character associated with this descriptor.
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// The descriptor's current connection state.
        /// </summary>
        public ConnectionState State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Sends the specified message to the client.
        /// </summary>
        /// <param name="message">The message to send to the client.</param>
        public async Task SendAsync(string message)
        {
            if (!IsConnected) return;

            var bytes = Encoding.UTF8.GetBytes(message);

            await Client.GetStream().WriteAsync(bytes);
        }

        /// <summary>
        /// Sends the specified message to the client.
        /// </summary>
        /// <param name="message">The message to send to the client.</param>
        public void Send(string message)
        {
            if (!IsConnected) return;

            var bytes = Encoding.UTF8.GetBytes(message);

            Client.GetStream().Write(bytes);
        }

        public async Task<string> ReadAsync()
        {
            var buffer = new byte[4096];

            if (!IsConnected) return null;

            var byteCount = await Client.GetStream().ReadAsync(buffer);

            var request = Encoding.UTF8.GetString(buffer, 0, byteCount);
            return request;
        }

        public string Read()
        {
            var buffer = new byte[4096];

            if (!IsConnected) return null;

            var byteCount = Client.GetStream().Read(buffer);

            var request = Encoding.UTF8.GetString(buffer, 0, byteCount);
            return request;
        }

        /// <summary>
        /// Disconnects the client and frees any resources allocated by this descriptor.
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected) return;

            Client.Close();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
