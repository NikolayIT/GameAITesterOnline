// <copyright file="201511291330516_GamesExecutorClassName.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GamesExecutorClassName : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Competitions", "GamesExecutorClassName", c => c.String());
        }

        public override void Down()
        {
            this.DropColumn("dbo.Competitions", "GamesExecutorClassName");
        }
    }
}
