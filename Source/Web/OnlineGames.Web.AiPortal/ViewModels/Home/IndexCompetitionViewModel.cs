namespace OnlineGames.Web.AiPortal.ViewModels.Home
{
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class IndexCompetitionViewModel : IMapFrom<Competition>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
