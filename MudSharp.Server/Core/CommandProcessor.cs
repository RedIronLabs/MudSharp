using System;
using System.Collections.Generic;
using System.Text;

namespace MudSharp.Server.Core
{
    public static class CommandProcessor
    {
        public static void ProcessCommand(string input)
        {
            Console.WriteLine("Process command: " + input);
        }
    }
}
