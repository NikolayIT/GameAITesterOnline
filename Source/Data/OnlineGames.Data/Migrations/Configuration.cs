namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using OnlineGames.Common;
    using OnlineGames.Data.Models;

    public sealed class Configuration : DbMigrationsConfiguration<AiPortalDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AiPortalDbContext context)
        {
            if (!context.Roles.Any())
            {
                this.SeedRoles(context);
                this.SeedCompetitions(context);

                context.SaveChanges();
            }
        }

        private void SeedRoles(AiPortalDbContext context)
        {
            var role = new Role { Name = GlobalConstants.AdministratorRoleName };
            context.Roles.Add(role);
        }

        private void SeedCompetitions(AiPortalDbContext context)
        {
            context.Competitions.Add(
                new Competition
                    {
                        Name = "Santase (Сантасе) AI",
                        Description = @"<p><strong>Santase</strong> (known as <strong>66</strong>, Сантасе, Sixty-six or <strong>Sechsundsechzig</strong>) is a well-known card game in Bulgaria and also played in Germany.</p>
<p>It is a fast <strong>6-card game</strong> for <strong>2 players</strong> played with a deck of 24 cards consisting of the <em>Ace</em>, <em>Ten</em>, <em>King</em>, <em>Queen</em>, <em>Jack</em> and <em>Nine</em>.</p>
<p>The game is named 66 because the objective of each hand is to be the first to collect 66 card points in tricks and melds. </p>",
                        IsActive = true,
                        MinimumParticipants = 2,
                        MaximumParticipants = 3,
                        LibraryValidatorClassName = "OnlineGames.Services.AiPortal.Uploads.LibraryValidators.SantaseLibraryValidator"
                });
            context.Competitions.Add(
                new Competition
                    {
                        Name = "Texas Hold'em AI",
                        Description = @"<p>As in most forms of poker, Texas Hold’em uses a standard 52-card deck that is shuffled before every hand.</p>
<p>Each player starts with two hole cards. There are three rounds of community cards. These are dealt face up, for every player to use, with betting after each round.</p>
<p>The best 5-card hand using any combination of the five community cards and two hole cards wins.</p>",
                        IsActive = true,
                        MinimumParticipants = 2,
                        MaximumParticipants = 3,
                        LibraryValidatorClassName = "OnlineGames.Services.AiPortal.Uploads.LibraryValidators.TexasHoldemLibraryValidator"
                });
        }
    }
}
