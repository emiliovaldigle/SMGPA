namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTaqsk10 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "DesplazamientoHoras");
            DropColumn("dbo.Tasks", "DesplazamientoDias");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "DesplazamientoDias", c => c.Double());
            AddColumn("dbo.Tasks", "DesplazamientoHoras", c => c.Double());
        }
    }
}
