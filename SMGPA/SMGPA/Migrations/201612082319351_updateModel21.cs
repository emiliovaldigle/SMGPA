namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModel21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "Activity_idActivity", "dbo.Activity");
            DropIndex("dbo.Tasks", new[] { "Activity_idActivity" });
            RenameColumn(table: "dbo.Tasks", name: "Activity_idActivity", newName: "idActivity");
            AlterColumn("dbo.Tasks", "idActivity", c => c.Guid(nullable: false));
            CreateIndex("dbo.Tasks", "idActivity");
            AddForeignKey("dbo.Tasks", "idActivity", "dbo.Activity", "idActivity", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "idActivity", "dbo.Activity");
            DropIndex("dbo.Tasks", new[] { "idActivity" });
            AlterColumn("dbo.Tasks", "idActivity", c => c.Guid());
            RenameColumn(table: "dbo.Tasks", name: "idActivity", newName: "Activity_idActivity");
            CreateIndex("dbo.Tasks", "Activity_idActivity");
            AddForeignKey("dbo.Tasks", "Activity_idActivity", "dbo.Activity", "idActivity");
        }
    }
}
