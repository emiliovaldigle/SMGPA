namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            AddColumn("dbo.Document", "idTask", c => c.Guid(nullable: false));
            AddColumn("dbo.Document", "Fecha", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Document", "idTask");
            AddForeignKey("dbo.Document", "idTask", "dbo.Tasks", "idTask", cascadeDelete: true);
            DropColumn("dbo.Tasks", "idDocument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "idDocument", c => c.Guid());
            DropForeignKey("dbo.Document", "idTask", "dbo.Tasks");
            DropIndex("dbo.Document", new[] { "idTask" });
            DropColumn("dbo.Document", "Fecha");
            DropColumn("dbo.Document", "idTask");
            CreateIndex("dbo.Tasks", "idDocument");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument");
        }
    }
}
