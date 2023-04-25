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
    public Invitation GetInvitation(Guid id)
    {
        return new Invitation
        {
            Id = Guid.NewGuid(),
            CanAttend = true,
            FoodAllergies = "can only eat meat",
            Message = "hellowwww"
        };
    }
}
