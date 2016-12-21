namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "Clase", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operation", "Clase");
        }
    }
}
