namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "MailInstitucional", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String(maxLength: 20));
            AlterColumn("dbo.User", "CorreoPersonal", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "CorreoPersonal", c => c.String());
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String());
            AlterColumn("dbo.User", "MailInstitucional", c => c.String(nullable: false));
        }
    }
}
