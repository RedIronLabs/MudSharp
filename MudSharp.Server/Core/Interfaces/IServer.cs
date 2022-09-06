namespace MudSharp.Server.Core
{
    internal interface IServer
    {
        void Listen();
        void Shutdown();
        void StartServer();
    }
}