// <copyright file="201511282136457_Battles.cs" company="Nikolay Kostov (Nikolay.IT)">
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
            this.CreateTable(
                "dbo.Battles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstTeamId = c.Int(nullable: false),
                        SecondTeamId = c.Int(nullable: false),
                        IsFinished = c.Boolean(nullable: false),
                        Comment = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.FirstTeamId, cascadeDelete: false)
                .ForeignKey("dbo.Teams", t => t.SecondTeamId, cascadeDelete: false)
                .Index(t => t.FirstTeamId)
                .Index(t => t.SecondTeamId)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.BattleGameResults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BattleId = c.Int(nullable: false),
                        Report = c.String(),
                        BattleGameWinner = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Battles", t => t.BattleId, cascadeDelete: true)
                .Index(t => t.BattleId)
                .Index(t => t.IsDeleted);

            this.AddColumn("dbo.Competitions", "GamesToPlayForEachBattle", c => c.Int(nullable: false));
            this.AddColumn("dbo.Teams", "Points", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.Battles", "SecondTeamId", "dbo.Teams");
            this.DropForeignKey("dbo.Battles", "FirstTeamId", "dbo.Teams");
            this.DropForeignKey("dbo.BattleGameResults", "BattleId", "dbo.Battles");
            this.DropIndex("dbo.BattleGameResults", new[] { "IsDeleted" });
            this.DropIndex("dbo.BattleGameResults", new[] { "BattleId" });
            this.DropIndex("dbo.Battles", new[] { "IsDeleted" });
            this.DropIndex("dbo.Battles", new[] { "SecondTeamId" });
            this.DropIndex("dbo.Battles", new[] { "FirstTeamId" });
            this.DropColumn("dbo.Teams", "Points");
            this.DropColumn("dbo.Competitions", "GamesToPlayForEachBattle");
            this.DropTable("dbo.BattleGameResults");
            this.DropTable("dbo.Battles");
        }
    }
}
