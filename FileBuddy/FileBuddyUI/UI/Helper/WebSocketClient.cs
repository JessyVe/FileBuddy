using FileBuddyUI.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocketServer.Client;
using WebSocketServer.MessageTypes;

namespace FileBuddyUI.UI.Helper
{
    public class WebSocketClient : PropertyChangedBase
    {
        private static readonly log4net.ILog Log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected; 
            set
            {
                if (_isConnected == value)
                    return;

                _isConnected = value;

                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private SocketClient _client;

        private Task<bool> _listenTask;
        private Task _updateTask;
        private Task _connectionTask;

        private DateTime _pingSent;
        private DateTime _pingLastSent;
        private bool _pinged = false;

        private static WebSocketClient _instance;

        public static WebSocketClient Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new WebSocketClient();

                return _instance;
            }
        }

        private WebSocketClient() {

            
        }

        public event EventHandler NewUpdateRequestReceived;

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = NewUpdateRequestReceived;
            handler?.Invoke(this, e);
        }

        public async Task Connect()
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

            }
            return true;
        }

        private async Task InitializeConnection(ClientMessage connectionPacket)
        {
            _pinged = false;

            if (IsConnected)
            {
                _updateTask = Task.Run(() => Update());
                await _client.SendObject(connectionPacket);
                _connectionTask = Task.Run(() => MonitorConnection());        
            }
            else
            {
                await Disconnect();
            }
        }

        private async Task<ClientMessage> GetNewConnectionPacket()
        {
            _listenTask = Task.Run(() => _client.ConnectToServer());

            IsConnected = await _listenTask;

            var notifyServer = new UserConnectionPacket
            {
                UserId = UserInformation.Instance.CurrentUser.Id,
                IsJoining = true,
                UserGuid = _client.UserId.ToString()
            };

            var personalPacket = new ClientMessage
            {
                SenderId = _client.UserId,
                Package = notifyServer
            };
            return personalPacket;
        }

        public async Task Disconnect()
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
                var recieved = await MonitorData();

                if (recieved)
                    Console.WriteLine(recieved);
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
                if (timePassed > TimeSpan.FromSeconds(5))
                {
                    if (!_pinged)
                    {
                        var result = await _client.PingConnection();
                        _pinged = true;

                        Thread.Sleep(5000);

                        if (_pinged)
                            await Task.Run(() => Disconnect());
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
            NewUpdateRequestReceived.Invoke(this, new EventArgs());

            if (packet != null)
            {
                if (packet is UpdateMessage chatP)
                {
                    NewUpdateRequestReceived.Invoke(this, new EventArgs());
                }

                if (packet is UserConnectionPacket connectionP)
                {
                   
                }

                if (packet is PingMessage pingP)
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
