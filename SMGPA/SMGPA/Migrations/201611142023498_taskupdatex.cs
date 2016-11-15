namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdatex : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "idEntities", "dbo.Entities");
            DropForeignKey("dbo.Tasks", "idFunctionary", "dbo.User");
            DropIndex("dbo.Tasks", new[] { "idFunctionary" });
            DropIndex("dbo.Tasks", new[] { "idEntities" });
            AlterColumn("dbo.Tasks", "TiempoInactividad", c => c.Double());
            AlterColumn("dbo.Tasks", "DesplazamientoHoras", c => c.Double());
            AlterColumn("dbo.Tasks", "DesplazamientoDias", c => c.Double());
            AlterColumn("dbo.Tasks", "idFunctionary", c => c.Guid());
            AlterColumn("dbo.Tasks", "idEntities", c => c.Guid());
            CreateIndex("dbo.Tasks", "idFunctionary");
            CreateIndex("dbo.Tasks", "idEntities");
            AddForeignKey("dbo.Tasks", "idEntities", "dbo.Entities", "idEntities");
            AddForeignKey("dbo.Tasks", "idFunctionary", "dbo.User", "idUser");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "idFunctionary", "dbo.User");
            DropForeignKey("dbo.Tasks", "idEntities", "dbo.Entities");
            DropIndex("dbo.Tasks", new[] { "idEntities" });
            DropIndex("dbo.Tasks", new[] { "idFunctionary" });
            AlterColumn("dbo.Tasks", "idEntities", c => c.Guid(nullable: false));
            AlterColumn("dbo.Tasks", "idFunctionary", c => c.Guid(nullable: false));
            AlterColumn("dbo.Tasks", "DesplazamientoDias", c => c.Double(nullable: false));
            AlterColumn("dbo.Tasks", "DesplazamientoHoras", c => c.Double(nullable: false));
            AlterColumn("dbo.Tasks", "TiempoInactividad", c => c.Double(nullable: false));
            CreateIndex("dbo.Tasks", "idEntities");
            CreateIndex("dbo.Tasks", "idFunctionary");
            AddForeignKey("dbo.Tasks", "idFunctionary", "dbo.User", "idUser", cascadeDelete: true);
            AddForeignKey("dbo.Tasks", "idEntities", "dbo.Entities", "idEntities", cascadeDelete: true);
        }
    }
}
