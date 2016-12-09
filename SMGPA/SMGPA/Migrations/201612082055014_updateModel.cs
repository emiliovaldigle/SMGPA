namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activity", "Nombre", c => c.String(nullable: false, maxLength: 90));
            AlterColumn("dbo.Entities", "Nombre", c => c.String(nullable: false, maxLength: 90));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entities", "Nombre", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Activity", "Nombre", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
