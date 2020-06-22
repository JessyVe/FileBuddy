using FileBuddyUI.Helper;
using FileBuddyUI.UI.Helper.CustomEventArgs;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WebSocketServer.Client;
using WebSocketServer.MessageTypes;

namespace FileBuddyUI.UI.Helper
{
    public class FileBuddyClient : IWebSocketClient
    {
        private static readonly log4net.ILog Log =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsConnected { get; set; }

        private SocketClient _client;
        private Task<bool> _listenTask;
        private Task _updateTask;
        private Task _connectionTask;

        private DateTime _pingSent;
        private DateTime _pingLastSent;
        private bool _pinged = false;

        private static FileBuddyClient _instance;
        public static FileBuddyClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileBuddyClient();

                return _instance;
            }
        }

        private FileBuddyClient()
        { }

        public event EventHandler NewUpdateRequestReceived;
        protected virtual void OnNewUpdateRequestReceived(UpdateRequestEventArgs e)
        {
            var handler = NewUpdateRequestReceived;
            handler?.Invoke(this, e);
        }

        public async Task ConnectToServer()
        {
            if (SetupClient())
            {
                var packet = await GetNewConnectionPacket();
                await InitializeConnection(packet);
            }
        }

        private bool SetupClient()
        {
            try
            {
                _client = new SocketClient(SettingsHelper.Instance.ApplicationSettings.SocketServerAddress,
                    SettingsHelper.Instance.ApplicationSettings.SocketServerPort, 100);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Connection to server could not be established.", ex);
            }
            return true;
        }

        private async Task<UserConnectionMessage> GetNewConnectionPacket()
        {
            _listenTask = Task.Run(() => _client.ConnectToServer());

            IsConnected = await _listenTask;

            return new UserConnectionMessage
            {
                UserId = UserInformation.Instance.CurrentUser.Id,
            };
        }

        private async Task InitializeConnection(UserConnectionMessage connectionPacket)
        {
            _pinged = false;

            if (IsConnected)
            {
                _updateTask = Task.Run(Update);
                await _client.SendObject(connectionPacket);
                _connectionTask = Task.Run(MonitorConnection);

                Log.Info("Connection to server was established successfully!");
                return;
            }
            await DisconnectFromServer();
        }

        public async Task DisconnectFromServer()
        {
            if (IsConnected)
            {
                IsConnected = false;
                await _connectionTask;
                await _updateTask;

                _client.Disconnect();
                Log.Info("Connection lost. Client was disconnected from server.");
            }
        }

        public async Task Send(int receiverId)
        {
            Log.Debug("Sending an update message to server. ");
            await _client.SendObject(new UpdateMessage()
            {
                ReceiverId = receiverId
            });
        }

        private async Task Update()
        {
            while (IsConnected)
            {
                Thread.Sleep(1);
                await MonitorData();
            }
        }

        private async Task MonitorConnection()
        {
            _pingSent = DateTime.Now;
            _pingLastSent = DateTime.Now;

            while (IsConnected)
            {
                var timePassed = (_pingSent.TimeOfDay - _pingLastSent.TimeOfDay);

                if (timePassed > TimeSpan.FromSeconds(20))
                {
                    if (!_pinged)
                    {
                        Log.Debug("Pinging server...");
                        _pingSent = DateTime.Now;

                        var result = await _client.PingConnection();  // send a ping request
                        _pinged = true; // ping was executed

                        Thread.Sleep(15000); // wait a pre-defined time for a response

                        if (!_pinged) 
                            continue;

                        Log.Debug("Ping was unsuccessful. Client will be disconnected.");
                        await Task.Run(DisconnectFromServer);
                    }
                }
            }
        }

        private async Task MonitorData()
        {
            var newObject = await _client.ReceiveMessage();

            try
            {
                Application.Current.Dispatcher.Invoke(() => ManagePacket(newObject));
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Main thread is currently not accessible.", ex);
            }
        }

        /// <summary>
        /// Handles received messages from server. 
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private bool ManagePacket(object packet)
        {
            switch (packet)
            {
                case null:
                    return false;
                case UpdateMessage _:
                    Log.Debug("New update message was received. System will be notified.");
                    OnNewUpdateRequestReceived(new UpdateRequestEventArgs());
                    break;
                case PingMessage _:
                    Log.Debug("Ping succeeded!");

                    _pingLastSent = DateTime.Now;
                    _pingSent = _pingLastSent;
                    _pinged = false;
                    break;
            }
            return true;
        }
    }
}
