namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "Responsable", c => c.Int(nullable: false));
            AddColumn("dbo.Operation", "Validable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "idResponsable", c => c.Guid());
            CreateIndex("dbo.Tasks", "idResponsable");
            AddForeignKey("dbo.Tasks", "idResponsable", "dbo.Entities", "idEntities");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "idResponsable", "dbo.Entities");
            DropIndex("dbo.Tasks", new[] { "idResponsable" });
            DropColumn("dbo.Tasks", "idResponsable");
            DropColumn("dbo.Operation", "Validable");
            DropColumn("dbo.Operation", "Responsable");
        }
    }
}
