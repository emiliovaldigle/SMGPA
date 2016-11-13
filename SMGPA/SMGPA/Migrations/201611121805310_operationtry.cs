namespace SMGPA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class operationtry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operation", "idPredecesora", c => c.Guid());
            CreateIndex("dbo.Operation", "idPredecesora");
            AddForeignKey("dbo.Operation", "idPredecesora", "dbo.Operation", "idOperation");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operation", "idPredecesora", "dbo.Operation");
            DropIndex("dbo.Operation", new[] { "idPredecesora" });
            DropColumn("dbo.Operation", "idPredecesora");
        }
    }
}
