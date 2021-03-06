﻿using log4net;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using WebSocketServer.Server;

namespace WebSocketServerUI
{
    public class Server
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string Port = "8000";
        private const string IpAddress = "127.0.0.1";

        private bool _isRunning;

        private ServerBase _socketServer;

        private Task _updateTask;
        private Task _listenTask;

        public static void Main(string[] args) 
        {
            var log4NetConfig = new XmlDocument();
            log4NetConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);

            Log.Info("*** Server will be available in short. Please wait ... ***");
            var server = new Server();
            server.StartServer();

            Console.Read();
        }

        private void StartServer()
        {
            SetupServer();

            _listenTask = Task.Run(() => _socketServer.StartServer());
            _updateTask = Task.Run(Update);
        }

        private void SetupServer()
        {
            Log.Info("Validating data...");
            var isValidPort = int.TryParse(Port, out var socketPort);

            if (!isValidPort)
            {
                Log.Error("Port value is not valid.");
                return;
            }
            Log.Info("Setting up server...");
            _socketServer = new SocketServer(IPAddress.Parse(IpAddress), socketPort);
            Log.Info($"Variables are initialized. The server is now online ({_socketServer.ConnectionInformation})");
        }

        private async Task Stop()
        {
            Log.Info("Server is stopping...");
            _isRunning = false;
            _socketServer.StopServer();

            await _listenTask;
            await _updateTask;

            Log.Info("*** Server has stopped ***\nPress any key to exit ...");
        }      

        private void Update()
        {
            while (_isRunning)
            {
                Thread.Sleep(5);
                if (!_socketServer.IsRunning)
                {
                    Task.Run(Stop);
                    return;
                }
                Log.Debug("Server is online...");
            }
        }
    }
}
