public interface ICosmosService
{
    public Task<Invitation> GetInvitation(string id);
    public Task InitialiseDatabase();
}
