namespace ShipShareAPI.Application.Interfaces.Token
{
    public interface ITokenHandler
    {
        Dto.Token.Token CreateAccessToken();
    }
}
