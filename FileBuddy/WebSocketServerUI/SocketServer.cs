using SharedRessources.Dtos.MessageTypes;
using System.Net;
using WebSocketServer.Client;
using WebSocketServer.Server;

namespace WebSocketServerUI
{
    public class SocketServer : ServerBase
    {
        public SocketServer(IPAddress address, int port) : base(address, port)
        { }

        protected override void ProcessClientPackets(object receivedMessage, SocketClient client)
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
