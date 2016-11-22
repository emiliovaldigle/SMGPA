namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTaskfinal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "TiempoInactividad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "TiempoInactividad", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
