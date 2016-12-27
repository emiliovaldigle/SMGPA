namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTaskModelOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "IteracionesPermitidas", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operation", "IteracionesPermitidas");
        }
    }
}
