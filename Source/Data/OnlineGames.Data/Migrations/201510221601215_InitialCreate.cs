﻿// <copyright file="201510221601215_InitialCreate.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Competitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        MinimumParticipants = c.Int(nullable: false),
                        MaximumParticipants = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CompetitionId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitions", t => t.CompetitionId, cascadeDelete: true)
                .Index(t => t.CompetitionId)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        AvatarUrl = c.String(),
                        Salt = c.String(),
                        PasswordHash = c.String(),
                        Provider = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_Id);
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.TeamMembers", "UserId", "dbo.Users");
            this.DropForeignKey("dbo.RoleUsers", "User_Id", "dbo.Users");
            this.DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            this.DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.Teams");
            this.DropForeignKey("dbo.Teams", "CompetitionId", "dbo.Competitions");
            this.DropIndex("dbo.RoleUsers", new[] { "User_Id" });
            this.DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            this.DropIndex("dbo.Roles", new[] { "IsDeleted" });
            this.DropIndex("dbo.Users", new[] { "IsDeleted" });
            this.DropIndex("dbo.TeamMembers", new[] { "IsDeleted" });
            this.DropIndex("dbo.TeamMembers", new[] { "UserId" });
            this.DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            this.DropIndex("dbo.Teams", new[] { "IsDeleted" });
            this.DropIndex("dbo.Teams", new[] { "CompetitionId" });
            this.DropIndex("dbo.Competitions", new[] { "IsDeleted" });
            this.DropTable("dbo.RoleUsers");
            this.DropTable("dbo.Roles");
            this.DropTable("dbo.Users");
            this.DropTable("dbo.TeamMembers");
            this.DropTable("dbo.Teams");
            this.DropTable("dbo.Competitions");
        }
    }
}
