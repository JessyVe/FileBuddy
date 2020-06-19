﻿using System;

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
