namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Operation", "OperacionResponsable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operation", "OperacionResponsable", c => c.Int(nullable: false));
            DropColumn("dbo.Operation", "Type");
        }
    }
}
