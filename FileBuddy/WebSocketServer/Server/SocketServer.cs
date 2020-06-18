using WebSocketServer.MessageTypes;
using System.Net;

namespace WebSocketServer.Server
{
    public class SocketServer : ServerBase
    {
        public SocketServer(IPAddress address, int port) : base(address, port)
        { }

        protected override void ProcessClientPackets(object receivedMessage, Client.SocketClient client)
        {
            if (receivedMessage is PingMessage pingPacket)
            {
                client.SendObject(pingPacket).Wait();
                return;
            }
            else
            {
               
            }
        }
    }
}
