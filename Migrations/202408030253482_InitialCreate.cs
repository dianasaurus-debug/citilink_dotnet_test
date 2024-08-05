namespace RestaurantBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GuestTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TableNumber = c.Int(nullable: false),
                        SeatCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reservation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReservationCode = c.String(),
                        GuestCount = c.Int(nullable: false),
                        ReservationTime = c.DateTime(nullable: false),
                        Notes = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        GuestTableId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.GuestTable", t => t.GuestTableId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.GuestTableId);
            
            CreateTable(
                "dbo.MenuCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        MenuType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                        MenuType = c.Int(nullable: false),
                        MenuCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MenuCategory", t => t.MenuCategoryId, cascadeDelete: true)
                .Index(t => t.MenuCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menu", "MenuCategoryId", "dbo.MenuCategory");
            DropForeignKey("dbo.Reservation", "GuestTableId", "dbo.GuestTable");
            DropForeignKey("dbo.Reservation", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Menu", new[] { "MenuCategoryId" });
            DropIndex("dbo.Reservation", new[] { "GuestTableId" });
            DropIndex("dbo.Reservation", new[] { "CustomerId" });
            DropTable("dbo.Menu");
            DropTable("dbo.MenuCategory");
            DropTable("dbo.Reservation");
            DropTable("dbo.GuestTable");
            DropTable("dbo.Customer");
            DropTable("dbo.Admin");
        }
    }
}
