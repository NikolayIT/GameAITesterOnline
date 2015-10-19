namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Teams",
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
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.TeamMembers", "UserId", "dbo.Users");
            this.DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.Teams");
            this.DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            this.DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            this.DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            this.DropIndex("dbo.UserRoles", new[] { "User_Id" });
            this.DropIndex("dbo.TeamMembers", new[] { "IsDeleted" });
            this.DropIndex("dbo.TeamMembers", new[] { "UserId" });
            this.DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            this.DropIndex("dbo.Teams", new[] { "IsDeleted" });
            this.DropIndex("dbo.Users", new[] { "IsDeleted" });
            this.DropIndex("dbo.Roles", new[] { "IsDeleted" });
            this.DropTable("dbo.UserRoles");
            this.DropTable("dbo.TeamMembers");
            this.DropTable("dbo.Teams");
            this.DropTable("dbo.Users");
            this.DropTable("dbo.Roles");
        }
    }
}
