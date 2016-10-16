namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        Rut = c.String(nullable: false),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        MailInstitucional = c.String(nullable: false),
                        Contrasena = c.String(nullable: false),
                        Carrera = c.String(),
                        NumeroTelefono = c.String(),
                        CorreoPersonal = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Operation",
                c => new
                    {
                        OperationId = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ActiveObserver = c.Boolean(nullable: false),
                        ProcessId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        date_start = c.DateTime(),
                        date_end = c.DateTime(),
                        status = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.OperationId)
                .ForeignKey("dbo.Process", t => t.ProcessId, cascadeDelete: true)
                .Index(t => t.ProcessId);
            
            CreateTable(
                "dbo.Process",
                c => new
                    {
                        ProcessId = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        state = c.Int(),
                        start_date = c.DateTime(),
                        end_date = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ProcessId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operation", "ProcessId", "dbo.Process");
            DropIndex("dbo.Operation", new[] { "ProcessId" });
            DropTable("dbo.Process");
            DropTable("dbo.Operation");
            DropTable("dbo.User");
        }
    }
}
