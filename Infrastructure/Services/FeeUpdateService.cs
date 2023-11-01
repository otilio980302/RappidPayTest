using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RapidPayTest.Infrastructure.Services
{
    public class FeeUpdateService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UfeService.Instance.UpdateFee();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                //await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
