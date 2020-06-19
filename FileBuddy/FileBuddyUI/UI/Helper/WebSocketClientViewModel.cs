using FileBuddyUI.Helper;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocketServer.Client;
using WebSocketServer.MessageTypes;

namespace FileBuddyUI.UI.Helper
{
    public class WebSocketClientViewModel : ViewModelBase
    {
        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;

                _status = value;

                OnPropertyChanged(nameof(Status));
            }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get => _isRunning; 
            set
            {
                if (_isRunning == value)
                    return;

                _isRunning = value;

                OnPropertyChanged(nameof(IsRunning));
            }
        }

        private SocketClient _client;

        private Task<bool> _listenTask;
        private Task _updateTask;
        private Task _connectionTask;

        private DateTime _pingSent;
        private DateTime _pingLastSent;
        private bool _pinged = false;

        public async Task Connect()
        {
            Status = "Connecting...";

            if (SetupClient())
            {
                var packet = await GetNewConnectionPacket();
                await InitializeConnection(packet);
            }
        }

        private bool SetupClient()
        {
            _client = new SocketClient(SettingsHelper.Instance.ApplicationSettings.SocketServerAddress, 
                SettingsHelper.Instance.ApplicationSettings.SocketServerPort, 100);
            return true;
        }

        private async Task InitializeConnection(ClientMessage connectionPacket)
        {
            _pinged = false;

            if (IsRunning)
            {
                _updateTask = Task.Run(() => Update());
                await _client.SendObject(connectionPacket);
                _connectionTask = Task.Run(() => MonitorConnection());
                Status = "Connected";
            }
            else
            {
                Status = "Connection failed";
                await Disconnect();
            }
        }

        private async Task<ClientMessage> GetNewConnectionPacket()
        {
            _listenTask = Task.Run(() => _client.ConnectToServer());

            IsRunning = await _listenTask;

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
            if (IsRunning)
            {
                IsRunning = false;
                await _connectionTask;
                await _updateTask;

                _client.Disconnect();
            }

            Status = "Disconnected";
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
            while (IsRunning)
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

            while (IsRunning)
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
            if (packet != null)
            {
                if (packet is UpdateMessage chatP)
                {
                   
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
