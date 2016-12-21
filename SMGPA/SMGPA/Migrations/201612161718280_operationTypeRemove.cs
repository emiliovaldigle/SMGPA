namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationTypeRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Operation", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operation", "Type", c => c.Int(nullable: false));
        }
    }
}
