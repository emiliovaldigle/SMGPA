namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
