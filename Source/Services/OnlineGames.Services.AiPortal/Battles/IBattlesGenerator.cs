namespace OnlineGames.Services.AiPortal.Battles
{
    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;

    public interface IBattlesGenerator
    {
        void GenerateBattles(IDbRepository<Team> teamsRepository, IDbRepository<Battle> battlesRepository, int competitionId);
    }
}
