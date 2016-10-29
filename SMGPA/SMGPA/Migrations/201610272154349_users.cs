namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Nombre_Apellido", c => c.String());
            DropColumn("dbo.User", "NombreApellido");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "NombreApellido", c => c.String());
            DropColumn("dbo.User", "Nombre_Apellido");
        }
    }
}
