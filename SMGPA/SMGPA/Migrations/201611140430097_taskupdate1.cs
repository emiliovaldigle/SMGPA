namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Tipo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Tipo");
        }
    }
}
