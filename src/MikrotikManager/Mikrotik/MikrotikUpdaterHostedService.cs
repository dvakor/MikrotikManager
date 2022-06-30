namespace MikrotikManager.Mikrotik
{
    public class MikrotikUpdaterHostedService : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly Scheduler<MikrotikManager> _scheduler;
        private readonly ILogger<MikrotikUpdaterHostedService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MikrotikUpdaterHostedService(IServiceProvider provider, Scheduler<MikrotikManager> scheduler, ILogger<MikrotikUpdaterHostedService> logger)
        {
            _provider = provider;
            _scheduler = scheduler;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _scheduler.NextTick(stoppingToken);
                
                if (stoppingToken.IsCancellationRequested) continue;
                
                _logger.LogInformation("Обновление маршрутов");
                
                await using var scope = _provider.CreateAsyncScope();
                var mikrotikService = scope.ServiceProvider.GetRequiredService<MikrotikManager>();

                try
                {
                    await mikrotikService.UpdateRoutes();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка обновления маршрутов");
                }
                
                _logger.LogInformation("Обновление завершено");
            }
        }
    }
}