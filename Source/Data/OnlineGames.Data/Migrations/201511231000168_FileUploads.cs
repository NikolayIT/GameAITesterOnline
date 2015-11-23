namespace OnlineGames.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FileUploads : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileContents = c.Binary(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                        Team_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id, cascadeDelete: true)
                .Index(t => t.IsDeleted)
                .Index(t => t.Team_Id);

            this.AddColumn("dbo.Competitions", "LibraryValidatorClassName", c => c.String());
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.Uploads", "Team_Id", "dbo.Teams");
            this.DropIndex("dbo.Uploads", new[] { "Team_Id" });
            this.DropIndex("dbo.Uploads", new[] { "IsDeleted" });
            this.DropColumn("dbo.Competitions", "LibraryValidatorClassName");
            this.DropTable("dbo.Uploads");
        }
    }
}
