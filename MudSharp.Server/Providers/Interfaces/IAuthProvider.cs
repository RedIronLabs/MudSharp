using System;
using System.Collections.Generic;
using System.Text;

namespace MudSharp.Server.Providers
{
    /// <summary>
    /// Authentication and authorization provider interface.
    /// </summary>
    internal interface IAuthProvider
    {
        /// <summary>
        /// Attempts to authenticate a user with the given username and password combination.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <param name="password">The password to check.</param>
        /// <returns><c>true</c> if the user authenticated successfully, otherwise <c>false</c>.</returns>
        bool Authenticate(string username, string password);
    }
}
