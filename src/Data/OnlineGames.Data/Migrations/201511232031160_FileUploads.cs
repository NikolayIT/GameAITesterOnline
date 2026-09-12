// <copyright file="201511232031160_FileUploads.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FileUploads : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Uploads",
                c =>
                new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        FileName = c.String(),
                        FileContents = c.Binary(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.IsDeleted);

            this.AddColumn("dbo.Competitions", "LibraryValidatorClassName", c => c.String());
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.Uploads", "TeamId", "dbo.Teams");
            this.DropIndex("dbo.Uploads", new[] { "IsDeleted" });
            this.DropIndex("dbo.Uploads", new[] { "TeamId" });
            this.DropColumn("dbo.Competitions", "LibraryValidatorClassName");
            this.DropTable("dbo.Uploads");
        }
    }
}
