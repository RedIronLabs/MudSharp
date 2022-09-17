using System;
using System.Collections.Generic;
using System.Text;

namespace MudSharp.Server.Providers
{
    public interface ILoggingProvider
    {
        void LogMessage(string message);
    }
}
