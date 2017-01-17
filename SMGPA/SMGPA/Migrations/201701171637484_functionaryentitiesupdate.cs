namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class functionaryentitiesupdate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntitiesFunctionary", newName: "FunctionaryEntities");
            DropPrimaryKey("dbo.FunctionaryEntities");
            CreateTable(
                "dbo.FunctionaryEntity",
                c => new
                    {
                        idUser = c.Guid(nullable: false),
                        idEntities = c.Guid(nullable: false),
                        Cargo = c.String(),
                    })
                .PrimaryKey(t => new { t.idUser, t.idEntities })
                .ForeignKey("dbo.Entities", t => t.idEntities, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.idUser, cascadeDelete: true)
                .Index(t => t.idUser)
                .Index(t => t.idEntities);
            
            AddColumn("dbo.Activity", "idCareer", c => c.Guid(nullable: false));
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String(maxLength: 8));
            AddPrimaryKey("dbo.FunctionaryEntities", new[] { "Functionary_idUser", "Entities_idEntities" });
            CreateIndex("dbo.Activity", "idCareer");
            AddForeignKey("dbo.Activity", "idCareer", "dbo.Career", "idCareer", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activity", "idCareer", "dbo.Career");
            DropForeignKey("dbo.FunctionaryEntity", "idUser", "dbo.User");
            DropForeignKey("dbo.FunctionaryEntity", "idEntities", "dbo.Entities");
            DropIndex("dbo.FunctionaryEntity", new[] { "idEntities" });
            DropIndex("dbo.FunctionaryEntity", new[] { "idUser" });
            DropIndex("dbo.Activity", new[] { "idCareer" });
            DropPrimaryKey("dbo.FunctionaryEntities");
            AlterColumn("dbo.User", "NumeroTelefono", c => c.String(maxLength: 15));
            DropColumn("dbo.Activity", "idCareer");
            DropTable("dbo.FunctionaryEntity");
            AddPrimaryKey("dbo.FunctionaryEntities", new[] { "Entities_idEntities", "Functionary_idUser" });
            RenameTable(name: "dbo.FunctionaryEntities", newName: "EntitiesFunctionary");
        }
    }
}
