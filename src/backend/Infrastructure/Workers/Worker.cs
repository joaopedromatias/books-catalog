using Application.Interfaces;
using Hangfire;

namespace Worker;

public class WorkerJob : BackgroundService
{
    private const string INGESTION_JOB_ID = "ingestion_job";
    private readonly ILogger<WorkerJob> _logger; 
    private readonly IIngestionJob _ingestionJob; 

    public WorkerJob(ILogger<WorkerJob> logger, IIngestionJob ingestionJob)
    {
        _logger = logger;
        _ingestionJob = ingestionJob;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Configuring worker service");

        RecurringJob.AddOrUpdate(
            INGESTION_JOB_ID,
            () => _ingestionJob.Start(cancellationToken),
            Cron.Hourly());

        RecurringJob.TriggerJob(INGESTION_JOB_ID);

        _logger.LogInformation("Worker service configured");

        using (var server = new BackgroundJobServer())
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
