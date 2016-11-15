namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
