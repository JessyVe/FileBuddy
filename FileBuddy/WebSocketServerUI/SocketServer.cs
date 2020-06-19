using System.Net;
using WebSocketServer.Client;
using WebSocketServer.MessageTypes;
using WebSocketServer.Server;

namespace WebSocketServerUI
{
    public class SocketServer : ServerBase
    {
        private static readonly log4net.ILog Log =
                 log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SocketServer(IPAddress address, int port) : base(address, port)
        { }

        /// <summary>
        /// Processes all user messages.
        /// </summary>
        /// <param name="receivedMessage"></param>
        /// <param name="client"></param>
        protected override void ProcessClientPackets(object receivedMessage, SocketClient client)
        {
            if(receivedMessage is UserConnectionMessage userConnectionMessage)
            {
                Log.Debug("userConnectionMessage was received. UserId will be updated.");
                client.UserId = userConnectionMessage.UserId;
            }
            else if (receivedMessage is PingMessage pingPacket)
            {
                Log.Debug("PingMessage was received. Answering client...");
                client.SendObject(pingPacket).Wait();
                Log.Debug("Ping succeeded!");
            }
            else if(receivedMessage is UpdateMessage updateMessage)
            {
                Log.Debug("UpdateMessage was received. Affected clients will be notified.");
                var user = Connections.Find(connection => connection.UserId == updateMessage.ReceiverId);

                if (user != null)
                {
                    client.SendObject(new UpdateMessage()).Wait();
                    Log.Debug("Successfully notified client.");
                }
                else
                {
                    Log.Debug("Client seems to be offline. Realtime update was not possible.");
                }
            }
        }
    }
}
