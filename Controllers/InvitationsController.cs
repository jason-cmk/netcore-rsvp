using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class InvitationsController : ControllerBase
{
    private readonly ILogger<InvitationsController> _logger;
    private readonly ICosmosService _cosmosService;

    public InvitationsController(ILogger<InvitationsController> logger, ICosmosService cosmosService)
    {
        _logger = logger;
        _cosmosService = cosmosService;
    }

    [HttpGet(Name = "GetInvitation")]
    [Route("{id}")]
    public async Task<Invitation> GetInvitation(string id)
    {
        await _cosmosService.InitialiseDatabase();
        var invitation = new Invitation
        {
            Id = id,
            Name = "Charlie",
            CanAttend = null,
            FoodAllergies = "can only eat meat",
            Message = "hellowwww"
        };
        
        _logger.LogInformation($"Getting invitation {{ Id: {invitation.Id}, Name: {invitation.Name} }}");

        return invitation;
    }
}
