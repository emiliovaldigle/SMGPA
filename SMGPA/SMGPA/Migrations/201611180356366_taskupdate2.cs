namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            DropTable("dbo.Document");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        idDocument = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.idDocument);
            
            CreateIndex("dbo.Tasks", "idDocument");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument");
        }
    }
}
