namespace RestaurantBooking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixReservationTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservation", "GuestTableId", "dbo.GuestTable");
            DropIndex("dbo.Reservation", new[] { "GuestTableId" });
            RenameColumn(table: "dbo.Reservation", name: "GuestTableId", newName: "GuestTable_ID");
            CreateTable(
                "dbo.ReservationTable",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GuestTableID = c.Int(nullable: false),
                        ReservationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GuestTable", t => t.GuestTableID, cascadeDelete: true)
                .ForeignKey("dbo.Reservation", t => t.ReservationID, cascadeDelete: true)
                .Index(t => t.GuestTableID)
                .Index(t => t.ReservationID);
            
            AlterColumn("dbo.Reservation", "GuestTable_ID", c => c.Int());
            CreateIndex("dbo.Reservation", "GuestTable_ID");
            AddForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservation", "GuestTable_ID", "dbo.GuestTable");
            DropForeignKey("dbo.ReservationTable", "ReservationID", "dbo.Reservation");
            DropForeignKey("dbo.ReservationTable", "GuestTableID", "dbo.GuestTable");
            DropIndex("dbo.ReservationTable", new[] { "ReservationID" });
            DropIndex("dbo.ReservationTable", new[] { "GuestTableID" });
            DropIndex("dbo.Reservation", new[] { "GuestTable_ID" });
            AlterColumn("dbo.Reservation", "GuestTable_ID", c => c.Int(nullable: false));
            DropTable("dbo.ReservationTable");
            RenameColumn(table: "dbo.Reservation", name: "GuestTable_ID", newName: "GuestTableId");
            CreateIndex("dbo.Reservation", "GuestTableId");
            AddForeignKey("dbo.Reservation", "GuestTableId", "dbo.GuestTable", "ID", cascadeDelete: true);
        }
    }
}
