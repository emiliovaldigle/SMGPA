namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entitiesUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entities", "Descripcion", c => c.String(nullable: false));
            DropColumn("dbo.Entities", "idFunctionary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Entities", "idFunctionary", c => c.Guid(nullable: false));
            DropColumn("dbo.Entities", "Descripcion");
        }
    }
}
