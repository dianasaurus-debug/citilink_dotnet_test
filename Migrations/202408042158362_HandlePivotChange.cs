namespace RestaurantBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HandlePivotChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReservationGuestTable", "Reservation_ID", "dbo.Reservation");
            DropForeignKey("dbo.ReservationGuestTable", "GuestTable_ID", "dbo.GuestTable");
            DropIndex("dbo.ReservationGuestTable", new[] { "Reservation_ID" });
            DropIndex("dbo.ReservationGuestTable", new[] { "GuestTable_ID" });
            AddColumn("dbo.Reservation", "GuestTable_ID", c => c.Int());
            CreateIndex("dbo.Reservation", "GuestTable_ID");
            AddForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable", "ID");
            DropTable("dbo.ReservationGuestTable");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReservationGuestTable",
                c => new
                    {
                        Reservation_ID = c.Int(nullable: false),
                        GuestTable_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reservation_ID, t.GuestTable_ID });
            
            DropForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable");
            DropIndex("dbo.Reservation", new[] { "GuestTable_ID" });
            DropColumn("dbo.Reservation", "GuestTable_ID");
            CreateIndex("dbo.ReservationGuestTable", "GuestTable_ID");
            CreateIndex("dbo.ReservationGuestTable", "Reservation_ID");
            AddForeignKey("dbo.ReservationGuestTable", "GuestTable_ID", "dbo.GuestTable", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ReservationGuestTable", "Reservation_ID", "dbo.Reservation", "ID", cascadeDelete: true);
        }
    }
}
