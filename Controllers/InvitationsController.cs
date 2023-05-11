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
        var invitation = await _cosmosService.GetInvitation(id);

        _logger.LogInformation($"Getting invitation {{ Id: {invitation.Id}, Name: {invitation.Name} }}");

        return invitation;
    }

    [HttpGet(Name = "InitialiseInvitations")]
    [Route("init")]
    public async Task Initialise()
    {
        await _cosmosService.InitialiseDatabase();
    }
}
