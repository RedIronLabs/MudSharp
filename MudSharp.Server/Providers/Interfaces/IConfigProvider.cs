using MudSharp.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MudSharp.Server.Providers
{
    internal interface IConfigProvider
    {
        CoreConfiguration Core { get; }
    }
}
