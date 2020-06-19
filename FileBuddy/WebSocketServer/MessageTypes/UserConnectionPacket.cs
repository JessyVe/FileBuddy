using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServer.MessageTypes
{
    [Serializable]
    public class UserConnectionPacket
    {
        public int UserId { get; set; }
        public string UserGuid { get; set; }
        public string[] Users { get; set; }
        public bool IsJoining { get; set; }
    }
}
