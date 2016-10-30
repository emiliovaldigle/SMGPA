namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCareer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "idCareer", "dbo.Career");
            DropPrimaryKey("dbo.Career");
            AddColumn("dbo.Career", "idCareer", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Career", "idCareer");
            AddForeignKey("dbo.User", "idCareer", "dbo.Career", "idCareer", cascadeDelete: true);
            DropColumn("dbo.Career", "idCarrera");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Career", "idCarrera", c => c.Guid(nullable: false, identity: true));
            DropForeignKey("dbo.User", "idCareer", "dbo.Career");
            DropPrimaryKey("dbo.Career");
            DropColumn("dbo.Career", "idCareer");
            AddPrimaryKey("dbo.Career", "idCarrera");
            AddForeignKey("dbo.User", "idCareer", "dbo.Career", "idCarrera", cascadeDelete: true);
        }
    }
}
