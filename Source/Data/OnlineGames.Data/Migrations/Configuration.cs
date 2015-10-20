namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    using OnlineGames.Common;
    using OnlineGames.Data.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<AiPortalDbContext>
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

                context.SaveChanges();
            }
        }

        private void SeedRoles(AiPortalDbContext context)
        {
            var role = new Role { Name = GlobalConstants.AdministratorRoleName };
            context.Roles.Add(role);
        }
    }
}
