namespace TODOWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ToDoItemDbContext : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoItems", "Tag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoItems", "Tag");
        }
    }
}
