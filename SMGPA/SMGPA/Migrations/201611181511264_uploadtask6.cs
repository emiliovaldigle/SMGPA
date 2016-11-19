namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadtask6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        idDocument = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.idDocument);
            
            AddColumn("dbo.Tasks", "idDocument", c => c.Guid());
            CreateIndex("dbo.Tasks", "idDocument");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument");
            DropColumn("dbo.Tasks", "Document");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Document", c => c.Binary());
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            DropColumn("dbo.Tasks", "idDocument");
            DropTable("dbo.Document");
        }
    }
}
