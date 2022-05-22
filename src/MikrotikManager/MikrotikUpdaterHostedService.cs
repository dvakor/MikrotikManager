namespace MikrotikManager
{
    public class MikrotikUpdaterHostedService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly Scheduler<MikrotikService> _scheduler;
        private readonly ILogger<MikrotikUpdaterHostedService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MikrotikUpdaterHostedService(IServiceProvider provider, Scheduler<MikrotikService> scheduler, ILogger<MikrotikUpdaterHostedService> logger)
        {
            _provider = provider;
            _scheduler = scheduler;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _provider.CreateAsyncScope();
            await using var mikrotikService = scope.ServiceProvider.GetRequiredService<MikrotikService>();
            while (!stoppingToken.IsCancellationRequested)
            {
                await _scheduler.NextTick(stoppingToken);
                
                if (stoppingToken.IsCancellationRequested) continue;
                
                _logger.LogInformation("Обновление маршрутов");
                await mikrotikService.UpdateRoutes();
            }
        }
    }
}