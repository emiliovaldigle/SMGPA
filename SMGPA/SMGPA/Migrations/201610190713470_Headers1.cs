namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Headers1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Process", "Descripcion", c => c.String(nullable: false));
            DropColumn("dbo.Process", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Process", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Process", "Descripcion");
        }
    }
}
