using FileBuddyUI.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        #region Singleton
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
        #endregion

        public event EventHandler NewUpdateRequestReceived;
        protected virtual void OnNewUpdateRequestReceived(EventArgs e)
        {
            EventHandler handler = NewUpdateRequestReceived;
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
                _updateTask = Task.Run(() => Update());
                await _client.SendObject(connectionPacket);
                _connectionTask = Task.Run(() => MonitorConnection());

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

                        Thread.Sleep(15000); // wait a pre-definied time for a response

                        if (_pinged) // _pinged should be rested to false by now (Method: ManagePacket)
                        {
                            Log.Debug("Ping was unsuccessfull. Client will be disconnected.");
                            await Task.Run(() => DisconnectFromServer());
                        }
                    }
                }
            }
        }

        private async Task<bool> MonitorData()
        {
            var newObject = await _client.ReceiveMessage();

            App.Current.Dispatcher.Invoke(delegate
            {
                return ManagePacket(newObject);
            });
            return false;
        }

        /// <summary>
        /// Handles recevied message from server. 
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private bool ManagePacket(object packet)
        {
            if (packet != null)
            {
                if (packet is UpdateMessage)
                {
                    Log.Debug("New update message was received. Event will be fired.");
                    OnNewUpdateRequestReceived(new EventArgs());
                }
                if (packet is PingMessage)
                {
                    Log.Debug("Ping succeeded!");
                    _pingLastSent = DateTime.Now;
                    _pingSent = _pingLastSent;
                    _pinged = false;
                }
                return true;
            }
            return false;
        }
    }
}
