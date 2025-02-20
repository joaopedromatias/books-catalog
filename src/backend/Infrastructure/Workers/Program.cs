using IoC;
using Hangfire;
using ApplicationJob;
using Application.Interfaces;

namespace Worker;

public class Program 
{ 
    public static void Main(string[] args) 
    { 
        var builder = Host.CreateApplicationBuilder(args);
        
        builder.Services.AddHostedService<WorkerJob>();
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddData(builder.Configuration);
        builder.Services.AddBackgroundJobs();

        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseColouredConsoleLogProvider()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseActivator(new HangfireJobActivator(builder.Services.BuildServiceProvider()));
    
        var host = builder.Build();

        host.Run();
    }
}

public class HangfireJobActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public HangfireJobActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type type)
    { 
        if (type == typeof(IngestionJob)) 
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<IngestionJob>>();
            var libraryClient = _serviceProvider.GetRequiredService<ILibraryClient>();
            var serviceScopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new IngestionJob(logger, libraryClient, serviceScopeFactory);
        }
        throw new Exception("Unexpected type being activated by Hangfire activator");
    }
}