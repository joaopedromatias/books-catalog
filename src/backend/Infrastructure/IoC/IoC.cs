using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Microsoft.Extensions.Configuration;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Data.Repositories;
using ExternalInterfaces.OpenLibrary.Clients;
using ApplicationJob;
using Data.Transaction;

namespace IoC;

public static class IoC
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration) 
    { 
        services.AddScoped<IBookService, BookService>();
        services.AddSingleton<IFilterValidationService, FilterValidationService>();
        services.AddHttpClient<ILibraryClient, OpenLibraryClient>(client => 
        { 
            var baseUrl = configuration["OpenLibrary:BaseUrl"];

            client.BaseAddress = new Uri(baseUrl!);
            client.Timeout = TimeSpan.FromSeconds(15);
        });
    }

    public static void AddData(this IServiceCollection services, IConfiguration configuration) 
    { 
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<BookContext>(options => 
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
        });
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddBackgroundJobs(this IServiceCollection services) 
    { 
        services.AddSingleton<IIngestionJob, IngestionJob>();
    }
}
