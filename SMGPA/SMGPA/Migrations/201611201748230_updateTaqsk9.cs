namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTaqsk9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tasks", "idPredecesora");
            RenameColumn(table: "dbo.Tasks", name: "Predecesora_idTask", newName: "idPredecesora");
            RenameIndex(table: "dbo.Tasks", name: "IX_Predecesora_idTask", newName: "IX_idPredecesora");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Tasks", name: "IX_idPredecesora", newName: "IX_Predecesora_idTask");
            RenameColumn(table: "dbo.Tasks", name: "idPredecesora", newName: "Predecesora_idTask");
            AddColumn("dbo.Tasks", "idPredecesora", c => c.Guid());
        }
    }
}
