using System;
using System.Threading;

namespace MudSharp.Server.Core
{
    internal class Connection
    {
        private readonly Descriptor _desc;
        public Connection(Descriptor desc)
        {
            _desc = desc;
            var thread = new Thread(HandleConnection);
            thread.Start();
        }

        #region Private Methods
        private void HandleConnection()
        {
            Login();
            
            while (true)
            {
                var input =  _desc.Read();
                Console.WriteLine(_desc.Id + ": " + input);
                _desc.Send("You wrote: " + input);
            }
        }

        private void Login()
        {
            _desc.State = ConnectionState.GetUsername;
            _desc.Send("Username (new for new account): ");
        }

        #endregion
    }
}

