namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faculty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Career", "idFaculty", c => c.Guid(nullable: false));
            AddColumn("dbo.Entities", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Career", "Nombre", c => c.String(nullable: false, maxLength: 80));
            CreateIndex("dbo.Career", "idFaculty");
            AddForeignKey("dbo.Career", "idFaculty", "dbo.Entities", "idEntities", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Career", "idFaculty", "dbo.Entities");
            DropIndex("dbo.Career", new[] { "idFaculty" });
            AlterColumn("dbo.Career", "Nombre", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Entities", "Discriminator");
            DropColumn("dbo.Career", "idFaculty");
        }
    }
}
