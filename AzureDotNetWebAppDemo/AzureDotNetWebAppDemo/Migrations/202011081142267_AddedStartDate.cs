namespace AzureDotNetWebAppDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStartDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoItems", "StartDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoItems", "StartDate");
        }
    }
}
