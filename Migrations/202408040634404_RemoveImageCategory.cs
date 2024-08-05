namespace RestaurantBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveImageCategory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MenuCategory", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuCategory", "ImagePath", c => c.String());
        }
    }
}
