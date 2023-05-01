using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class InvitationsController : ControllerBase
{
    private readonly ILogger<InvitationsController> _logger;

    public InvitationsController(ILogger<InvitationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetInvitation")]
    [Route("{id}")]
    public Invitation GetInvitation(Guid id)
    {
        var invitation = new Invitation
        {
            Id = id,
            Name = "Charlie",
            CanAttend = true,
            FoodAllergies = "can only eat meat",
            Message = "hellowwww"
        };
        
        _logger.LogInformation($"Getting invitation {{ Id: {invitation.Id}, Name: {invitation.Name} }}");

        return invitation;
    }
}
