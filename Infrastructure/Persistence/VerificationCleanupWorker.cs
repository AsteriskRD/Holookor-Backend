using HolookorBackend.Core.Application.Interfaces.Repositories;

namespace HolookorBackend.Infrastructure.Persistence
{
    public class VerificationCleanupWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public VerificationCleanupWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IEmailVerificationRepo>();

                await repo.DeleteExpired(DateTime.UtcNow.AddMinutes(-30));
                await repo.SaveAsync();

                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }
    }

}
