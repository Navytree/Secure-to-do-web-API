namespace ToDoUserWebAPI.Models
{
    public interface ITokenService
    {
        string CreateJwtToken(User user);
    }

}
