namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "Nombre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Nombre", c => c.String());
        }
    }
}
