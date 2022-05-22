namespace MikrotikManager
{
    public class Scheduler<T>
    {
        private readonly TimeSpan _defaultPeriod = TimeSpan.FromMinutes(10);
        private readonly TimeSpan _period;
        private CancellationTokenSource _cts;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Scheduler(IConfiguration configuration)
        {
            var key = $"Scheduler:{typeof(T).Name}";
            var stringPeriod = configuration.GetValue<string>(key);
            _period = string.IsNullOrEmpty(stringPeriod) ? _defaultPeriod : TimeSpan.Parse(stringPeriod);
            _cts = new CancellationTokenSource();
            _cts.Cancel();
        }

        public void Reset()
        {
            if (!_cts.IsCancellationRequested)
                _cts.Cancel();
        }
        
        public async Task NextTick(CancellationToken stoppingToken)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, stoppingToken);
            try
            {
                await Task.Delay(_period, cts.Token);
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                _cts = new CancellationTokenSource(_period);
            }
        }
    }
}