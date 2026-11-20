//using InsuranceApp.Application.Interfaces.Repositories;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace InsuranceApp.Infrastructure.BackgroundJobs;

//public class PolicyExpiryHostedService(
//    IServiceScopeFactory scopeFactory,
//    ILogger<PolicyExpiryHostedService> logger) : BackgroundService
//{
//    private const string JobName = "PolicyExpiry";

//    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
//    private readonly ILogger<PolicyExpiryHostedService> _logger = logger;
//    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Bucharest");

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        _logger.LogInformation("{Job} hosted service started.", JobName);

//        while (!stoppingToken.IsCancellationRequested)
//        {
//            var nowUtc = DateTime.UtcNow;
//            var nowLocal = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, _timeZone);


//            var nextMidnightLocal = nowLocal.Date.AddDays(1);
//            var nextMidnightUtc = TimeZoneInfo.ConvertTimeToUtc(nextMidnightLocal, _timeZone);

//            var delay = nextMidnightUtc - nowUtc;
//            if (delay < TimeSpan.Zero)
//                delay = TimeSpan.Zero;

//            _logger.LogInformation(
//                "{Job} waiting {Delay} until next run at {NextRunLocal} (local) / {NextRunUtc} (UTC).",
//                JobName,
//                delay,
//                nextMidnightLocal,
//                nextMidnightUtc);

//            try
//            {
//                await Task.Delay(delay, stoppingToken);
//            }
//            catch (TaskCanceledException)
//            {
//                break;
//            }

//            if (stoppingToken.IsCancellationRequested)
//                break;

//            try
//            {
//                using var scope = _scopeFactory.CreateScope();
//                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

//                var nowJobUtc = DateTime.UtcNow;

//                var toExpire = await unitOfWork.Policies.GetPoliciesToExpireAsync(nowJobUtc, stoppingToken);

//                foreach (var p in toExpire)
//                    p.Expire();

//                if (toExpire.Count > 0)
//                    await unitOfWork.Policies.UpdateRangeAsync(toExpire, stoppingToken);

//                await unitOfWork.SaveChangesAsync(stoppingToken);

//                _logger.LogInformation(
//                    "{Job} completed. Expired {Count} policies.",
//                    JobName,
//                    toExpire.Count);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "{Job} failed.", JobName);
//            }
//        }

//        _logger.LogInformation("{Job} hosted service stopping.", JobName);
//    }
//}

using InsuranceApp.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InsuranceApp.Infrastructure.BackgroundJobs;

public class PolicyExpiryHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<PolicyExpiryHostedService> logger) : BackgroundService
{
    private const string JobName = "PolicyExpiry";

    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<PolicyExpiryHostedService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{Job} hosted service started.", JobName);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var nowJobUtc = DateTime.UtcNow;

            var toExpire = await unitOfWork.Policies
                .GetPoliciesToExpireAsync(nowJobUtc, stoppingToken);

            foreach (var p in toExpire)
                p.Expire();

            if (toExpire.Count > 0)
                await unitOfWork.Policies.UpdateRangeAsync(toExpire, stoppingToken);

            await unitOfWork.SaveChangesAsync(stoppingToken);

            _logger.LogInformation(
                "{Job} completed. Expired {Count} policies.",
                JobName,
                toExpire.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Job} failed.", JobName);
        }

        _logger.LogInformation("{Job} hosted service finished.", JobName);
    }
}