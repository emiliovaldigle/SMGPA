namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faculty2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Career", "idFaculty", "dbo.Entities");
            DropIndex("dbo.Career", new[] { "idFaculty" });
            AlterColumn("dbo.Career", "idFaculty", c => c.Guid());
            CreateIndex("dbo.Career", "idFaculty");
            AddForeignKey("dbo.Career", "idFaculty", "dbo.Entities", "idEntities");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Career", "idFaculty", "dbo.Entities");
            DropIndex("dbo.Career", new[] { "idFaculty" });
            AlterColumn("dbo.Career", "idFaculty", c => c.Guid(nullable: false));
            CreateIndex("dbo.Career", "idFaculty");
            AddForeignKey("dbo.Career", "idFaculty", "dbo.Entities", "idEntities", cascadeDelete: true);
        }
    }
}
