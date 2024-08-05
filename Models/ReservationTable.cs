using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBooking.Models
{
    public class ReservationTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public required int GuestTableID { get; set; }
        [ForeignKey("GuestTableID")]
        public virtual GuestTable? GuestTable { get; set; }
        public required int ReservationID { get; set; }
        [ForeignKey("ReservationID")]
        public virtual Reservation? Reservation { get; set; }
    }
}
