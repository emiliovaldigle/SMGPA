namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        idActivity = c.Guid(nullable: false, identity: true),
                        state = c.Int(nullable: false),
                        start_date = c.DateTime(nullable: false),
                        end_date = c.DateTime(nullable: false),
                        idProcess = c.Guid(nullable: false),
                        idTask = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idActivity)
                .ForeignKey("dbo.Process", t => t.idProcess, cascadeDelete: true)
                .Index(t => t.idProcess);
            
            CreateTable(
                "dbo.Process",
                c => new
                    {
                        idProcess = c.Guid(nullable: false, identity: true),
                        Criterio = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idProcess);
            
            CreateTable(
                "dbo.Operation",
                c => new
                    {
                        idOperation = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        idProcess = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idOperation)
                .ForeignKey("dbo.Process", t => t.idProcess, cascadeDelete: true)
                .Index(t => t.idProcess);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        idTask = c.Guid(nullable: false, identity: true),
                        fechaInicio = c.DateTime(),
                        fechaFin = c.DateTime(),
                        TiempoInactividad = c.Double(nullable: false),
                        Desplazamiento = c.Double(nullable: false),
                        Documento = c.String(),
                        Estado = c.Int(nullable: false),
                        idFunctionary = c.Guid(nullable: false),
                        idEntities = c.Guid(nullable: false),
                        idObservation = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idTask)
                .ForeignKey("dbo.Entities", t => t.idEntities, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.idFunctionary, cascadeDelete: true)
                .ForeignKey("dbo.Activity", t => t.idTask)
                .Index(t => t.idTask)
                .Index(t => t.idFunctionary)
                .Index(t => t.idEntities);
            
            CreateTable(
                "dbo.Observation",
                c => new
                    {
                        idObservation = c.Guid(nullable: false, identity: true),
                        FechaComentario = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        ValidacionEstatus = c.Boolean(nullable: false),
                        idUser = c.Guid(nullable: false),
                        idTask = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idObservation)
                .ForeignKey("dbo.User", t => t.idUser, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.idObservation)
                .Index(t => t.idObservation)
                .Index(t => t.idUser);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        idUser = c.Guid(nullable: false, identity: true),
                        Rut = c.String(nullable: false),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Nombre_Apellido = c.String(),
                        MailInstitucional = c.String(nullable: false),
                        Contrasena = c.String(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        NumeroTelefono = c.String(),
                        CorreoPersonal = c.String(),
                        idCareer = c.Guid(),
                        idEntitie = c.Guid(),
                        idRole = c.Guid(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.idUser)
                .ForeignKey("dbo.Career", t => t.idCareer, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.idRole, cascadeDelete: true)
                .Index(t => t.idCareer)
                .Index(t => t.idRole);
            
            CreateTable(
                "dbo.Career",
                c => new
                    {
                        idCarrera = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Descripción = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idCarrera);
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        idEntities = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        idFunctionary = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idEntities);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        idRole = c.Guid(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Descripcion = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.idRole);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        idPermission = c.Guid(nullable: false, identity: true),
                        TextLink = c.String(nullable: false),
                        Controller = c.String(nullable: false),
                        ActionResult = c.String(nullable: false),
                        ActiveMenu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.idPermission);
            
            CreateTable(
                "dbo.EntitiesFunctionary",
                c => new
                    {
                        Entities_idEntities = c.Guid(nullable: false),
                        Functionary_idUser = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Entities_idEntities, t.Functionary_idUser })
                .ForeignKey("dbo.Entities", t => t.Entities_idEntities, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.Functionary_idUser, cascadeDelete: true)
                .Index(t => t.Entities_idEntities)
                .Index(t => t.Functionary_idUser);
            
            CreateTable(
                "dbo.PermissionRole",
                c => new
                    {
                        Permission_idPermission = c.Guid(nullable: false),
                        Role_idRole = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_idPermission, t.Role_idRole })
                .ForeignKey("dbo.Permission", t => t.Permission_idPermission, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.Role_idRole, cascadeDelete: true)
                .Index(t => t.Permission_idPermission)
                .Index(t => t.Role_idRole);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User", "idRole", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Role_idRole", "dbo.Role");
            DropForeignKey("dbo.PermissionRole", "Permission_idPermission", "dbo.Permission");
            DropForeignKey("dbo.Tasks", "idTask", "dbo.Activity");
            DropForeignKey("dbo.Tasks", "idFunctionary", "dbo.User");
            DropForeignKey("dbo.Tasks", "idEntities", "dbo.Entities");
            DropForeignKey("dbo.Observation", "idObservation", "dbo.Tasks");
            DropForeignKey("dbo.Observation", "idUser", "dbo.User");
            DropForeignKey("dbo.EntitiesFunctionary", "Functionary_idUser", "dbo.User");
            DropForeignKey("dbo.EntitiesFunctionary", "Entities_idEntities", "dbo.Entities");
            DropForeignKey("dbo.User", "idCareer", "dbo.Career");
            DropForeignKey("dbo.Activity", "idProcess", "dbo.Process");
            DropForeignKey("dbo.Operation", "idProcess", "dbo.Process");
            DropIndex("dbo.PermissionRole", new[] { "Role_idRole" });
            DropIndex("dbo.PermissionRole", new[] { "Permission_idPermission" });
            DropIndex("dbo.EntitiesFunctionary", new[] { "Functionary_idUser" });
            DropIndex("dbo.EntitiesFunctionary", new[] { "Entities_idEntities" });
            DropIndex("dbo.User", new[] { "idRole" });
            DropIndex("dbo.User", new[] { "idCareer" });
            DropIndex("dbo.Observation", new[] { "idUser" });
            DropIndex("dbo.Observation", new[] { "idObservation" });
            DropIndex("dbo.Tasks", new[] { "idEntities" });
            DropIndex("dbo.Tasks", new[] { "idFunctionary" });
            DropIndex("dbo.Tasks", new[] { "idTask" });
            DropIndex("dbo.Operation", new[] { "idProcess" });
            DropIndex("dbo.Activity", new[] { "idProcess" });
            DropTable("dbo.PermissionRole");
            DropTable("dbo.EntitiesFunctionary");
            DropTable("dbo.Permission");
            DropTable("dbo.Role");
            DropTable("dbo.Entities");
            DropTable("dbo.Career");
            DropTable("dbo.User");
            DropTable("dbo.Observation");
            DropTable("dbo.Tasks");
            DropTable("dbo.Operation");
            DropTable("dbo.Process");
            DropTable("dbo.Activity");
        }
    }
}
