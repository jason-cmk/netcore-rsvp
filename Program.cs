using Microsoft.Azure.Cosmos;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// Add logging provider
builder.Logging.ClearProviders();

ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton(logger);

// Add API Services 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
const string defaultCorsPolicy = "defaultCorsPolicy";
builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: defaultCorsPolicy,
                    policy =>
                    {
                        policy.WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
        });

// Configure Cosmos
var cosmosConfigSection = builder.Configuration.GetSection("CosmosConfig");

builder.Services.Configure<CosmosConfig>(cosmosConfigSection);

builder.Services.AddTransient<ICosmosService, CosmosService>();

builder.Services.AddSingleton<CosmosClient>(serviceProvider =>
{
    var cosmosConfig = cosmosConfigSection.Get<CosmosConfig>();

    if (cosmosConfig == null)
    {
        throw new RsvpException("Invalid cosmos configuartion section");
    }

    string? endpointUrl = cosmosConfig.EndpointUrl;
    string? cosmosPrimaryKey = cosmosConfig.CosmosPrimaryKey;
    string? dbName = cosmosConfig.DbName;

    if (endpointUrl != null && cosmosPrimaryKey != null)
    {
        return new CosmosClient(endpointUrl, cosmosPrimaryKey, new CosmosClientOptions() { ApplicationName = "CosmosDBDotnetQuickstart" });
    }

    throw new RsvpException("Unable to initialise Cosmos client.");
});

// Start application
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

app.UseCors(defaultCorsPolicy);

app.MapHealthChecks("/ping");

app.Run();

