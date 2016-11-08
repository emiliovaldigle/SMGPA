namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModel2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activity", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Operation", "Descripcion", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Observation", "Comentario", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.User", "Rut", c => c.String(nullable: false, maxLength: 14));
            AlterColumn("dbo.User", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String(maxLength: 15));
            AlterColumn("dbo.Career", "Nombre", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Entities", "Nombre", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entities", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Career", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String(maxLength: 20));
            AlterColumn("dbo.User", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.User", "Rut", c => c.String(nullable: false));
            AlterColumn("dbo.Observation", "Comentario", c => c.String(nullable: false));
            AlterColumn("dbo.Operation", "Descripcion", c => c.String(nullable: false));
            AlterColumn("dbo.Operation", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Activity", "Nombre", c => c.String());
        }
    }
}
