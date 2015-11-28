// <copyright file="201511281917526_Battles.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Battles : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Competitions", "GamesToPlayForEachBattle", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            this.DropColumn("dbo.Competitions", "GamesToPlayForEachBattle");
        }
    }
}
