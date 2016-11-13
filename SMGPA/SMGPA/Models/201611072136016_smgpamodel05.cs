namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smgpamodel05 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Observation", "idObservation", "dbo.Tasks");
            DropForeignKey("dbo.Observation", "idUser", "dbo.User");
            DropIndex("dbo.Observation", new[] { "idObservation" });
            DropIndex("dbo.Observation", new[] { "idUser" });
            RenameColumn(table: "dbo.Observation", name: "idUser", newName: "Funcionario_idUser");
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        idDocument = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.idDocument);
            
            AddColumn("dbo.Tasks", "idDocument", c => c.Guid(nullable: false));
            AddColumn("dbo.Observation", "Tarea_idTask", c => c.Guid());
            AddColumn("dbo.Career", "Descripcion", c => c.String(nullable: false));
            AddColumn("dbo.Career", "Activa", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Process", "Criterio", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Process", "Descripcion", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Observation", "Comentario", c => c.String(nullable: false));
            AlterColumn("dbo.Observation", "ValidacionEstatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Observation", "Funcionario_idUser", c => c.Guid());
            AlterColumn("dbo.Role", "Nombre", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Role", "Descripcion", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Tasks", "idDocument");
            CreateIndex("dbo.Observation", "Funcionario_idUser");
            CreateIndex("dbo.Observation", "Tarea_idTask");
            AddForeignKey("dbo.Tasks", "idDocument", "dbo.Document", "idDocument", cascadeDelete: true);
            AddForeignKey("dbo.Observation", "Tarea_idTask", "dbo.Tasks", "idTask");
            AddForeignKey("dbo.Observation", "Funcionario_idUser", "dbo.User", "idUser");
            DropColumn("dbo.Tasks", "Documento");
            DropColumn("dbo.Tasks", "idObservation");
            DropColumn("dbo.Observation", "idTask");
            DropColumn("dbo.Career", "Descripción");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Career", "Descripción", c => c.String(nullable: false));
            AddColumn("dbo.Observation", "idTask", c => c.Guid(nullable: false));
            AddColumn("dbo.Tasks", "idObservation", c => c.Guid(nullable: false));
            AddColumn("dbo.Tasks", "Documento", c => c.String());
            DropForeignKey("dbo.Observation", "Funcionario_idUser", "dbo.User");
            DropForeignKey("dbo.Observation", "Tarea_idTask", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "idDocument", "dbo.Document");
            DropIndex("dbo.Observation", new[] { "Tarea_idTask" });
            DropIndex("dbo.Observation", new[] { "Funcionario_idUser" });
            DropIndex("dbo.Tasks", new[] { "idDocument" });
            AlterColumn("dbo.Role", "Descripcion", c => c.String(nullable: false));
            AlterColumn("dbo.Role", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Observation", "Funcionario_idUser", c => c.Guid(nullable: false));
            AlterColumn("dbo.Observation", "ValidacionEstatus", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Observation", "Comentario", c => c.String());
            AlterColumn("dbo.Process", "Descripcion", c => c.String(nullable: false));
            AlterColumn("dbo.Process", "Criterio", c => c.String(nullable: false));
            DropColumn("dbo.Career", "Activa");
            DropColumn("dbo.Career", "Descripcion");
            DropColumn("dbo.Observation", "Tarea_idTask");
            DropColumn("dbo.Tasks", "idDocument");
            DropTable("dbo.Document");
            RenameColumn(table: "dbo.Observation", name: "Funcionario_idUser", newName: "idUser");
            CreateIndex("dbo.Observation", "idUser");
            CreateIndex("dbo.Observation", "idObservation");
            AddForeignKey("dbo.Observation", "idUser", "dbo.User", "idUser", cascadeDelete: true);
            AddForeignKey("dbo.Observation", "idObservation", "dbo.Tasks", "idTask");
        }
    }
}
