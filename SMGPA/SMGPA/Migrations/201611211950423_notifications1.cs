namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifications1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notificacion", "Fecha", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notificacion", "Fecha", c => c.String());
        }
    }
}
