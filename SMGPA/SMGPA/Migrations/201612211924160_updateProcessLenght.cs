namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProcessLenght : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Process", "Criterio", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Process", "Criterio", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
