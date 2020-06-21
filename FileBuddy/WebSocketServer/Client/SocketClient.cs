using WebSocketServer.MessageTypes;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WebSocketServer.Client
{
    /// <summary>
    /// Represents a client implementation for communcation 
    /// with a web socket server. 
    /// </summary>
    public class SocketClient
    {
        private static readonly log4net.ILog Log =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int UserId { get; set; }
        public Socket Socket { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public bool IsGuidAssigned { get; set; }

        public SocketClient(IPAddress ipAddress, int port, int userId)
        {
            UserId = userId;
            EndPoint = new IPEndPoint(ipAddress, port);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public SocketClient()
        { }

        public async Task<bool> ConnectToServer()
        {
            var result = await Task.Run(() => TryConnect());
            string guid = string.Empty;

            try
            {
                if (result)
                {
                    IsGuidAssigned = true;
                    return true;
                }
            }
            catch (SocketException ex)
            {
                Log.ErrorFormat("Unable to connect to server. ", ex);
            }
            return false;
        }

        public async Task<string> CreateGuid(Socket socket)
        {
            return await Task.Run(() => TryCreateGuid(socket));
        }

        public async Task<bool> SendMessage(string message)
        {
            return await Task.Run(() => TrySendMessage(message));
        }

        public async Task<bool> SendObject(object obj)
        {
            return await Task.Run(() => TrySendObject(obj));
        }

        public async Task<object> ReceiveMessage()
        {
            return await Task.Run(() => TryReceiveMessage());
        }

        private object TryReceiveMessage()
        {
            if (Socket.Available == 0)
                return null;

            byte[] data = new byte[Socket.ReceiveBufferSize];

            try
            {
                using (var networkStream = new NetworkStream(Socket))
                {
                    networkStream.Read(data, 0, data.Length);
                    var memory = new MemoryStream(data) { Position = 0 };
                    var formatter = new BinaryFormatter();
                    var obj = formatter.Deserialize(memory);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unable to receive message. ", ex);
                return null;
            }
        }

        private bool TrySendObject(object obj)
        {
            try
            {
                using (var networkStream = new NetworkStream(Socket))
                {
                    var memory = new MemoryStream();
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(memory, obj);

                    var newObj = memory.ToArray();
                    memory.Position = 0;

                    networkStream.Write(newObj, 0, newObj.Length);
                    return true;
                }
            }
            catch (IOException ex)
            {
                Log.ErrorFormat("Unable to send message to server. ", ex);
                return false;
            }
        }

        public bool TrySendMessage(string message)
        {
            try
            {
                using (var networkStream = new NetworkStream(Socket))
                {
                    var writer = new StreamWriter(networkStream) { AutoFlush = true };
                    writer.WriteLine(message);

                    return true;
                }
            }
            catch (IOException ex)
            {
                Log.ErrorFormat("Unable to send message to server. ", ex);
                return false;
            }
        }

        private bool TryConnect()
        {
            try
            {
                Socket.Connect(EndPoint);
                return true;
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unable to connect to server. ", ex);
                return false;
            }
        }

        public string ReceiveGuid()
        {
            try
            {
                using (var networkStream = new NetworkStream(Socket))
                {
                    var reader = new StreamReader(networkStream);
                    networkStream.ReadTimeout = 5000;

                    return reader.ReadLine();
                }
            }
            catch (IOException ex)
            {
                Log.ErrorFormat("Unable to receive guid from server. ", ex);
                return null;
            }
        }

        private string TryCreateGuid(Socket socket)
        {
            Socket = socket;
            var endPoint = (IPEndPoint)Socket.LocalEndPoint;
            EndPoint = endPoint;

            return UserId.ToString();
        }

        public bool IsSocketConnected()
        {
            try
            {
                return !(Socket.Poll(5000, SelectMode.SelectRead) && Socket.Available == 0);
            }
            catch (ObjectDisposedException ex)
            {
                Log.ErrorFormat("Connection to server lost. Socket not longer connected. ", ex);
                return false;
            }
        }

        public async Task<bool> PingConnection()
        {
            try
            {
                return await SendObject(new PingMessage());
            }
            catch (ObjectDisposedException ex)
            {
                Log.ErrorFormat("Ping to server failed. ", ex);
                return false;
            }
        }

        public void Disconnect()
        {
            Socket.Close();
        }
    }
}
