namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifications2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notificacion", "UrlAction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notificacion", "UrlAction");
        }
    }
}
