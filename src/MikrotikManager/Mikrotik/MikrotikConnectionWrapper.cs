using DanilovSoft.MikroApi;

namespace MikrotikManager.Mikrotik
{
    public sealed class MikrotikConnectionWrapper: IMikroTikConnection, IDisposable
    {
        private readonly ILogger<MikrotikConnectionWrapper> _logger;
        private IMikroTikConnection _connection;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MikrotikConnectionWrapper(ILogger<MikrotikConnectionWrapper> logger)
        {
            _logger = logger;
            _connection = new MikroTikConnection();
        }
        
        public void Connect(string login, string password, string hostname, bool useSsl, int port = 8728)
        {
            _connection.Connect(login, password, hostname, useSsl, port);
        }

        public void Connect(string login, string password, string hostname, bool useSsl, int port = 8728,
            RouterOsVersion version = RouterOsVersion.PostVersion6Dot43)
        {
            _logger.LogInformation("Connecting to {Host}:{Port}", hostname, port);
            _connection.Connect(login, password, hostname, useSsl, port, version);
        }

        public Task ConnectAsync(string login, string password, string hostname, bool useSsl, int port = 8728,
            CancellationToken cancellationToken = new())
        {
            _logger.LogInformation("Connecting to {Host}:{Port}", hostname, port);
            return _connection.ConnectAsync(login, password, hostname, useSsl, port, cancellationToken);
        }

        public Task ConnectAsync(string login, string password, string hostname, bool useSsl, int port = 8728,
            RouterOsVersion version = RouterOsVersion.PostVersion6Dot43, CancellationToken cancellationToken = new())
        {
            return _connection.ConnectAsync(login, password, hostname, useSsl, port, version, cancellationToken);
        }

        public void CancelListeners()
        {
            _connection.CancelListeners();
        }

        public void CancelListeners(bool wait)
        {
            _connection.CancelListeners(wait);
        }

        public Task CancelListenersAsync()
        {
            return _connection.CancelListenersAsync();
        }

        public Task CancelListenersAsync(bool wait)
        {
            return _connection.CancelListenersAsync(wait);
        }

        public MikroTikFlowCommand Command(string command)
        {
            
            return new MikroTikFlowCommand(command, this);
        }

        public void Dispose()
        {
            if (_connection.Connected)
            {
                _connection.Quit(1000);
            }
            _connection.Dispose();
        }

        public MikroTikResponseListener Listen(MikroTikCommand command)
        {
            return _connection.Listen(command);
        }

        public Task<MikroTikResponseListener> ListenAsync(MikroTikCommand command)
        {
            return _connection.ListenAsync(command);
        }

        public bool Quit(int millisecondsTimeout)
        {
            _logger.LogInformation("Connection closed");
            return _connection.Quit(millisecondsTimeout);
        }

        public Task<bool> QuitAsync(int millisecondsTimeout)
        {
            _logger.LogInformation("Connection closed");
            return _connection.QuitAsync(millisecondsTimeout);
        }

        public MikroTikResponse Send(MikroTikCommand command)
        {
            _logger.LogInformation("Sending command: {Command}", command);
            return _connection.Send(command);
        }

        public Task<MikroTikResponse> SendAsync(MikroTikCommand command)
        {
            _logger.LogInformation("Sending command: {Command}", command);
            return _connection.SendAsync(command);
        }

        public bool Connected => _connection.Connected;

        public int ReceiveTimeout
        {
            get => _connection.ReceiveTimeout;
            set => _connection.ReceiveTimeout = value;
        }

        public int SendTimeout
        {
            get => _connection.SendTimeout;
            set => _connection.SendTimeout = value;
        }
    }
}