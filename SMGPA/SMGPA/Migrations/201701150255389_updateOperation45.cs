namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOperation45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "PorcentajeAceptacion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operation", "PorcentajeAceptacion");
        }
    }
}
