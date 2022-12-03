using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;

namespace Rembot.Main.Services
{
    internal class PollingService<TReceiver> : BackgroundService where TReceiver : IUpdateReceiver
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PollingService<TReceiver>> _logger;

        public PollingService(IServiceProvider serviceProvider, ILogger<PollingService<TReceiver>> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receiving..");
            await StartReceiving(cancellationToken);
        }

        private async Task StartReceiving(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var receiver = scope.ServiceProvider.GetRequiredService<TReceiver>();
                    var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
                    await receiver.ReceiveAsync(updateHandler, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Polling failed with exception: {Exception}", ex);
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            }
        }
    }
}
