namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_update1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "Contrasena", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Contrasena", c => c.String(nullable: false, maxLength: 40));
        }
    }
}
