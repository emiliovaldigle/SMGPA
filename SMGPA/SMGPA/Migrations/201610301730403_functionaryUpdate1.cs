namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class functionaryUpdate1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String());
        }
    }
}
