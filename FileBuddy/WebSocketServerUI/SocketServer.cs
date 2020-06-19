using System.Net;
using WebSocketServer.Client;
using WebSocketServer.MessageTypes;
using WebSocketServer.Server;

namespace WebSocketServerUI
{
    public class SocketServer : ServerBase
    {
        public SocketServer(IPAddress address, int port) : base(address, port)
        { }

        /// <summary>
        /// Processes all user requestes.
        /// </summary>
        /// <param name="receivedMessage"></param>
        /// <param name="client"></param>
        protected override void ProcessClientPackets(object receivedMessage, SocketClient client)
        {
            if(receivedMessage is UserConnectionMessage userConnectionPacket)
            {
                client.UserId = userConnectionPacket.UserId;
            }
            else if (receivedMessage is PingMessage pingPacket)
            {
                client.SendObject(pingPacket).Wait();
            }
            else if(receivedMessage is UpdateMessage updateMessage)
            {
                var user = Connections.Find(connection => connection.UserId == updateMessage.ReceiverId);
                if (user != null)
                    client.SendObject(new UpdateMessage()).Wait();
            }
        }
    }
}
