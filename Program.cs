using Microsoft.Azure.Cosmos;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.ClearProviders();

ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var cosmosConfigSection = builder.Configuration.GetSection("CosmosConfig");
builder.Services.Configure<CosmosConfig>(cosmosConfigSection);

builder.Services.AddTransient<ICosmosService, CosmosService>();

const string localCorsPolicy = "local";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: localCorsPolicy,
            policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
            }
        );
});

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

    string? endpointUrl = config.GetValue<string>("CosmosConfig:EndpointUrl");
    string? cosmosPrimaryKey = config.GetValue<string>("CosmosConfig:CosmosPrimaryKey");
    string? dbName = config.GetValue<string>("CosmosConfig:DbName");

    if (endpointUrl != null && cosmosPrimaryKey != null)
    {
        return new CosmosClient(endpointUrl, cosmosPrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
    }

    throw new RsvpException("Unable to initialise Cosmos client.");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(localCorsPolicy);

app.Run();

