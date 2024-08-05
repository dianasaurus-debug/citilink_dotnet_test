using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBooking.Models
{
    public class MenuCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required MenuType MenuType { get; set; }
        public virtual ICollection<Menu>? Menus { get; set; }

    }
}
