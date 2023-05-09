using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

public class CosmosService : ICosmosService
{
    private readonly string? _dbName;
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger _logger;
    private readonly IOptions<CosmosConfig> _cosmosConfig;

    public CosmosService(ILogger<CosmosService> logger,
            IOptions<CosmosConfig> CosmosConfig,
            CosmosClient cosmosClient)
    {
        _logger = logger;
        _cosmosConfig = CosmosConfig;
        _cosmosClient = cosmosClient;
    }

    public async Task InitialiseDatabase()
    {
        _logger.LogInformation("Creating database and container if none exists");

        var dbName = _cosmosConfig.Value.DbName;
        var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        var database = databaseResponse.Database;

        var containerName = _cosmosConfig.Value.ContainerName;
        var containerResponse = await database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");
        var container = containerResponse.Container;

        await PopulateData(container);
    }

    private async Task PopulateData(Container container)
    {
        var invitation = new Invitation
        {
            Id = "27a1efe8-aace-404f-888a-4d8995a30e0f",
            CanAttend = true,
            Name = "Charlie Kang",
            FoodAllergies = "Too big fish",
            Message = "Meow meow :3"
        };

        await container.CreateItemAsync(invitation);
        _logger.LogInformation("Data populated");
    }
}
