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
            }
        }

        public async Task Send(int receiverId)
        {
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
                Thread.Sleep(1);
                var timePassed = (_pingSent.TimeOfDay - _pingLastSent.TimeOfDay);
                if (timePassed > TimeSpan.FromSeconds(10))
                {
                    if (!_pinged)
                    {
                        var result = await _client.PingConnection();
                        _pinged = true;

                        Thread.Sleep(5000);

                        if (!_pinged)
                            await Task.Run(() => DisconnectFromServer());
                    }
                }
                else
                {
                    _pingSent = DateTime.Now;
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

        private bool ManagePacket(object packet)
        {
            if (packet != null)
            {
                if (packet is UpdateMessage)
                {
                    OnNewUpdateRequestReceived(new EventArgs());
                }

                if (packet is PingMessage)
                {
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
