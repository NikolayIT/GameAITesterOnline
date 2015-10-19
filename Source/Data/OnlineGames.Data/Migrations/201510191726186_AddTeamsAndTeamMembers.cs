namespace OnlineGames.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTeamsAndTeamMembers : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedById = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.IsDeleted);

            this.CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.TeamId)
                .Index(t => t.UserId)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.TeamMembers", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.Teams");
            this.DropForeignKey("dbo.Teams", "CreatedById", "dbo.AspNetUsers");
            this.DropIndex("dbo.TeamMembers", new[] { "IsDeleted" });
            this.DropIndex("dbo.TeamMembers", new[] { "UserId" });
            this.DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            this.DropIndex("dbo.Teams", new[] { "IsDeleted" });
            this.DropIndex("dbo.Teams", new[] { "CreatedById" });
            this.DropTable("dbo.TeamMembers");
            this.DropTable("dbo.Teams");
        }
    }
}
