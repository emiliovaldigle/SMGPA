namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationUpdate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "idOperation", c => c.Guid());
            CreateIndex("dbo.Tasks", "idOperation");
            AddForeignKey("dbo.Tasks", "idOperation", "dbo.Operation", "idOperation");
            DropColumn("dbo.Tasks", "Descripcion");
            DropColumn("dbo.Tasks", "Tipo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "Descripcion", c => c.String());
            DropForeignKey("dbo.Tasks", "idOperation", "dbo.Operation");
            DropIndex("dbo.Tasks", new[] { "idOperation" });
            DropColumn("dbo.Tasks", "idOperation");
        }
    }
}
