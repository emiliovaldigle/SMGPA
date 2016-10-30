namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "Nombre_Apellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Nombre_Apellido", c => c.String());
        }
    }
}
