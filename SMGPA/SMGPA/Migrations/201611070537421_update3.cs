namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "idTask", "dbo.Activity");
            DropIndex("dbo.Tasks", new[] { "idTask" });
            AddColumn("dbo.Activity", "Nombre", c => c.String());
            AddColumn("dbo.Tasks", "Nombre", c => c.String());
            AddColumn("dbo.Tasks", "Descripcion", c => c.String());
            AddColumn("dbo.Tasks", "DesplazamientoHoras", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "DesplazamientoDias", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "Activity_idActivity", c => c.Guid());
            AlterColumn("dbo.Activity", "start_date", c => c.DateTime());
            AlterColumn("dbo.Activity", "end_date", c => c.DateTime());
            CreateIndex("dbo.Tasks", "Activity_idActivity");
            AddForeignKey("dbo.Tasks", "Activity_idActivity", "dbo.Activity", "idActivity");
            DropColumn("dbo.Activity", "idTask");
            DropColumn("dbo.Tasks", "Desplazamiento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Desplazamiento", c => c.Double(nullable: false));
            AddColumn("dbo.Activity", "idTask", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Tasks", "Activity_idActivity", "dbo.Activity");
            DropIndex("dbo.Tasks", new[] { "Activity_idActivity" });
            AlterColumn("dbo.Activity", "end_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Activity", "start_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tasks", "Activity_idActivity");
            DropColumn("dbo.Tasks", "DesplazamientoDias");
            DropColumn("dbo.Tasks", "DesplazamientoHoras");
            DropColumn("dbo.Tasks", "Descripcion");
            DropColumn("dbo.Tasks", "Nombre");
            DropColumn("dbo.Activity", "Nombre");
            CreateIndex("dbo.Tasks", "idTask");
            AddForeignKey("dbo.Tasks", "idTask", "dbo.Activity", "idActivity");
        }
    }
}
