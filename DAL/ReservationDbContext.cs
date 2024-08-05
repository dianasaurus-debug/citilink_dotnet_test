using Microsoft.Extensions.Hosting;
using RestaurantBooking.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace RestaurantBooking.DAL
{
    public class ReservationDbContext: DbContext
    {
        public ReservationDbContext() : base("ReservationDbContext")
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<GuestTable> GuestTables { get; set; }

        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<ReservationTable> ReservationTables { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
