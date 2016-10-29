namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "NombreApellido", c => c.String());
            DropColumn("dbo.User", "Nombre_Apellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Nombre_Apellido", c => c.String());
            DropColumn("dbo.User", "NombreApellido");
        }
    }
}
