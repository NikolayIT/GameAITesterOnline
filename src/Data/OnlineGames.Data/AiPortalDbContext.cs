// <copyright file="AiPortalDbContext.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using OnlineGames.Data.Common.Models;
    using OnlineGames.Data.Models;

    public class AiPortalDbContext : DbContext
    {
        public AiPortalDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Upload> Uploads { get; set; }

        public DbSet<Battle> Battles { get; set; }

        public DbSet<BattleGameResult> BattleGameResults { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
