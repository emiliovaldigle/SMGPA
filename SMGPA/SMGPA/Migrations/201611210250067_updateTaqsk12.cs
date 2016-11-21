namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTaqsk12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "TiempoInactividad", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "TiempoInactividad", c => c.Double());
        }
    }
}
