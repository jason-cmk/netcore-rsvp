using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class CosmosService : ICosmosService
{
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

    private const string _partitionKey = "/partitionKey";

    private Container GetContainer()
    {
        var dbName = _cosmosConfig.Value.DbName;
        var containerName = _cosmosConfig.Value.ContainerName;

        var container = _cosmosClient.GetContainer(dbName, containerName);

        return container;
    }

    public async Task<Invitation> GetInvitation(string id)
    {
        var container = GetContainer();

        var invitation = await container.ReadItemAsync<Invitation>(id, PartitionKey.None);

        return invitation;
    }

    public async Task InitialiseDatabase()
    {
        _logger.LogInformation("Creating database and container if none exists");

        var dbName = _cosmosConfig.Value.DbName;
        var databaseResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        var database = databaseResponse.Database;

        var containerName = _cosmosConfig.Value.ContainerName;
        var containerResponse = await database.CreateContainerIfNotExistsAsync(containerName, _partitionKey);
        var container = containerResponse.Container;

        await PopulateData(container);
    }

    public async Task<Invitation> UpdateInvitation(Invitation updateInvitationRequest)
    {
        var container = GetContainer();

        var updateInvitationResponse = await container.ReplaceItemAsync<Invitation>(updateInvitationRequest, updateInvitationRequest.Id, PartitionKey.None);

        return updateInvitationResponse;
    }

    private async Task PopulateData(Container container)
    {
        var inputText = File.ReadAllText(@".\Data\data.json");
        var invitations = JsonConvert.DeserializeObject<IList<Invitation>>(inputText);

        _logger.LogInformation("Attempting to create invitations:\n {Invitations}", JsonConvert.SerializeObject(invitations));

        if (invitations == null)
        {
            throw new RsvpException("Unable to serialize");
        }

        var tasks = new List<Task>();

        foreach (var invitation in invitations)
        {
            var task = Task.Run(async () => await container.CreateItemAsync(invitation));
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}
