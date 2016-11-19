namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "idDocument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "idDocument", c => c.Guid());
        }
    }
}
