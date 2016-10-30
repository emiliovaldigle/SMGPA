namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class functionaryUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "idEntities");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "idEntities", c => c.Guid());
        }
    }
}
