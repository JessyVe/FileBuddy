using System;
using System.Threading.Tasks;

namespace WebSocketServer.Client
{
    public interface IWebSocketClient
    {
        bool IsConnected { get; set; }

        event EventHandler NewUpdateRequestReceived;

        Task ConnectToServer();
        Task DisconnectFromServer();
        Task Send(int receiverId);
    }
}