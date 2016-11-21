namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskupdate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "idPredecesora", c => c.Guid());
            AddColumn("dbo.Tasks", "Predecesora_idTask", c => c.Guid());
            CreateIndex("dbo.Tasks", "Predecesora_idTask");
            AddForeignKey("dbo.Tasks", "Predecesora_idTask", "dbo.Tasks", "idTask");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Predecesora_idTask", "dbo.Tasks");
            DropIndex("dbo.Tasks", new[] { "Predecesora_idTask" });
            DropColumn("dbo.Tasks", "Predecesora_idTask");
            DropColumn("dbo.Tasks", "idPredecesora");
        }
    }
}
