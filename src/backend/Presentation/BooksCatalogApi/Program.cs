using IoC;

namespace BooksCatalogApi;

public class Program 
{ 
    public static void Main(string[] args) 
    { 
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var corsPolicy = "AllowAll";
        builder.Services.AddCors(options => 
        { 
            options.AddPolicy(corsPolicy, policy => 
            { 
                policy.AllowAnyOrigin().WithMethods("GET").AllowAnyHeader();
            });
        });

        builder.Services.AddData(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseCors(corsPolicy);
        app.UseHttpsRedirection();
        app.MapControllers();         

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => 
            { 
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.Run();
    }
}