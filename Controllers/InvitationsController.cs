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

        _logger.LogInformation("Getting invitation { Id: {Invitation}, Name: {Invitation} }", invitation.Id, invitation.Name);

        return invitation;
    }

    [HttpPut(Name = "UpdateInvitation")]
    public async Task<Invitation> UpdateInvitation(Invitation invitationRequest)
    {
        var invitation = await _cosmosService.UpdateInvitation(invitationRequest);

        _logger.LogInformation("Updated invitation { Id: {Invitation}, Name: {Invitation} }", invitation.Id, invitation.Name);
        
        return invitation;
    }


    [HttpGet(Name = "InitialiseInvitations")]
    [Route("init")]
    public async Task Initialise()
    {
        await _cosmosService.InitialiseDatabase();
    }
}
