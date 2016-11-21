namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTaqsk11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Reprogramaciones", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Reprogramaciones");
        }
    }
}
