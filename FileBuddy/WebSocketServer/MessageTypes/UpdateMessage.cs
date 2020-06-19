using System;

namespace WebSocketServer.MessageTypes
{
    [Serializable]
    public class UpdateMessage
    {
        public int ReceiverId { get; set; }
    }
}
