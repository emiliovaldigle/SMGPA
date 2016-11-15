namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            AlterColumn("dbo.Tasks", "idDocument", c => c.Guid());
            CreateIndex("dbo.Tasks", "idDocument");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            AlterColumn("dbo.Tasks", "idDocument", c => c.Guid(nullable: false));
            CreateIndex("dbo.Tasks", "idDocument");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument", cascadeDelete: true);
        }
    }
}
