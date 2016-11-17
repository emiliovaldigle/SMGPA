namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTask1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "idCareer", "dbo.Career");
            AddForeignKey("dbo.User", "idCareer", "dbo.Career", "idCareer");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "idCareer", "dbo.Career");
            AddForeignKey("dbo.User", "idCareer", "dbo.Career", "idCareer", cascadeDelete: true);
        }
    }
}
