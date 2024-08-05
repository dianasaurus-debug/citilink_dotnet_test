namespace RestaurantBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HandleSomeChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable");
            DropIndex("dbo.Reservation", new[] { "GuestTable_ID" });
            CreateTable(
                "dbo.ReservationGuestTable",
                c => new
                    {
                        Reservation_ID = c.Int(nullable: false),
                        GuestTable_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reservation_ID, t.GuestTable_ID })
                .ForeignKey("dbo.Reservation", t => t.Reservation_ID, cascadeDelete: true)
                .ForeignKey("dbo.GuestTable", t => t.GuestTable_ID, cascadeDelete: true)
                .Index(t => t.Reservation_ID)
                .Index(t => t.GuestTable_ID);
            
            DropColumn("dbo.Reservation", "GuestTable_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "GuestTable_ID", c => c.Int());
            DropForeignKey("dbo.ReservationGuestTable", "GuestTable_ID", "dbo.GuestTable");
            DropForeignKey("dbo.ReservationGuestTable", "Reservation_ID", "dbo.Reservation");
            DropIndex("dbo.ReservationGuestTable", new[] { "GuestTable_ID" });
            DropIndex("dbo.ReservationGuestTable", new[] { "Reservation_ID" });
            DropTable("dbo.ReservationGuestTable");
            CreateIndex("dbo.Reservation", "GuestTable_ID");
            AddForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable", "ID");
        }
    }
}
