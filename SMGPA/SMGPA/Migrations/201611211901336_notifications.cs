namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notificacion",
                c => new
                    {
                        idNotification = c.Guid(nullable: false, identity: true),
                        Cuerpo = c.String(),
                        Fecha = c.String(),
                        Vista = c.Boolean(nullable: false),
                        idUser = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.idNotification)
                .ForeignKey("dbo.User", t => t.idUser, cascadeDelete: true)
                .Index(t => t.idUser);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notificacion", "idUser", "dbo.User");
            DropIndex("dbo.Notificacion", new[] { "idUser" });
            DropTable("dbo.Notificacion");
        }
    }
}
