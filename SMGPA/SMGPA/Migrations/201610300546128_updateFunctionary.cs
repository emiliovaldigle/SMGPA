namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFunctionary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "idEntities", c => c.Guid());
            DropColumn("dbo.User", "idEntitie");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "idEntitie", c => c.Guid());
            DropColumn("dbo.User", "idEntities");
        }
    }
}
