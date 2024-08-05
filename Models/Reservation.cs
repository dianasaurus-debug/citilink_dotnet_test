using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBooking.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public required string ReservationCode { get; set; }
        public required int GuestCount { get; set; }
        public required DateTime ReservationTime { get; set; }
        public string? Notes { get; set; }
        public required bool IsValid { get; set; }

        public required int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<ReservationTable>? ReservationTables { get; set; }

    }
}
