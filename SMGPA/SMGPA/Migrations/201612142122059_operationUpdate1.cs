namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "OperacionResponsable", c => c.Int(nullable: false));
            DropColumn("dbo.Operation", "Responsable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operation", "Responsable", c => c.Int(nullable: false));
            DropColumn("dbo.Operation", "OperacionResponsable");
        }
    }
}
