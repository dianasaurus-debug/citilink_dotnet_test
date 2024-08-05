using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBooking.Models
{
    public class GuestTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public required int TableNumber { get; set; }
        public required int SeatCount { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }

    }
}
