using WebSocketServer.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace WebSocketServer.Server
{
    /// <summary>
    /// Implements main functionalities for a web socket server. 
    /// </summary>
    public abstract class ServerBase
    {
        protected static readonly log4net.ILog Log =
                 log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected List<SocketClient> Connections { get; set; }

        private IPEndPoint EndPoint { get; set; }
        private Socket Socket { get; set; }

        private Task _receivingTask;

        public bool IsRunning { get; private set; }  

        public string ConnectionInformation { get; private set; }

        protected ServerBase(IPAddress address, int port)
        {
            EndPoint = new IPEndPoint(address, port);
            Connections = new List<SocketClient>();

            SetupSocket();
        }

        protected abstract void ProcessClientPackets(object receivedMessage, SocketClient client);

        private void SetupSocket()
        {          
            try
            {
                Socket = new Socket(AddressFamily.InterNetwork,
                         SocketType.Stream, ProtocolType.Tcp)
                { ReceiveTimeout = 5000 };

                Socket.Bind(EndPoint);
                Socket.Listen(10);
                ConnectionInformation = $"{EndPoint.Address}:{EndPoint.Port}";

            } catch(SocketException ex)
            {
                Log.ErrorFormat("Socket address may already be in use.", ex);
            }
        }

        public async Task StartServer()
        {
            _receivingTask = Task.Run(() => MonitorConnections());
            IsRunning = true;

            await AcceptNewConnectionRequest();
            await _receivingTask;

            Socket.Close();
        }

        public void StopServer()
        {
            IsRunning = false;
            Connections.Clear();
        }

        /// <summary>
        /// Handles connection requests from clients.
        /// </summary>
        /// <returns></returns>
        public async Task AcceptNewConnectionRequest()
        {
            while (IsRunning)
            {
                if (Socket.Poll(100000, SelectMode.SelectRead))
                {
                    try
                    {
                        var newConnection = Socket.Accept();
                        {
                            var client = new SocketClient();
                            var newGuid = await client.CreateGuid(newConnection);
                            await client.SendMessage(newGuid);
                            Connections.Add(client);
                            Log.Info("A new connection was established.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("Requested connection could not be established.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Handles connection losses and received messages.
        /// </summary>
        private void MonitorConnections()
        {
            while (IsRunning)
            {
                foreach (var client in Connections.ToList())
                {
                    // remove client from collection if connection is lost
                    if (!client.IsSocketConnected())
                    {
                        Connections.Remove(client);
                        Log.Info("A connection was now longer valid and was removed. ");
                    }
                    // handle client message if received
                    else if (client.Socket.Available != 0)
                    {
                        Log.Debug("A new message was received from client. ");
                        var receivedMessage = ReadMessage(client.Socket);

                        if(receivedMessage != null)
                            ProcessClientPackets(receivedMessage, client);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the object, which was received by the server
        /// from the given socket.
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        protected object ReadMessage(Socket clientSocket)
        {
            var data = new byte[clientSocket.ReceiveBufferSize];

            try
            {
                using var networkStream = new NetworkStream(clientSocket);
                networkStream.Read(data, 0, data.Length);
                var memory = new MemoryStream(data) { Position = 0 };
                var formatter = new BinaryFormatter();

                return formatter.Deserialize(memory);
            } catch(Exception ex)
            {
                Log.ErrorFormat("Error while reading message from client. Request can not be processed. ", ex);
            }
            return null;
        }
    }
}
