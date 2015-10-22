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
                        IsActive = true,
                        MinimumParticipants = 2,
                        MaximumParticipants = 3
                    });
            context.Competitions.Add(
                new Competition
                    {
                        Name = "Texas Hold'em AI",
                        IsActive = true,
                        MinimumParticipants = 2,
                        MaximumParticipants = 3
                    });
        }
    }
}
