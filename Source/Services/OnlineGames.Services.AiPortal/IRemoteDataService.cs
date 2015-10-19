namespace OnlineGames.Services.AiPortal
{
    using System.Threading.Tasks;

    using OnlineGames.Services.AiPortal.Models;

    public interface IRemoteDataService
    {
        Task<CheckUserLoginApiModel> Login(string username, string password);
    }
}
