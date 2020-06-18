using System;

namespace WebSocketServer.MessageTypes
{
    [Serializable]
    public class ClientMessage
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public object Package { get; set; }
    }
}
