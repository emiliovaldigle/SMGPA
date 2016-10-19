namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Headers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "Nombre", c => c.String(nullable: false));
            AddColumn("dbo.Operation", "Descripcion", c => c.String(nullable: false));
            DropColumn("dbo.Operation", "Name");
            DropColumn("dbo.Operation", "Descripcion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operation", "Descripcion", c => c.String(nullable: false));
            AddColumn("dbo.Operation", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Operation", "Descripcion");
            DropColumn("dbo.Operation", "Nombre");
        }
    }
}
