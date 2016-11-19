namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadtask5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "Document", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "Document");
        }
    }
}
