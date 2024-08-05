using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBooking.Models
{
    public enum MenuType
    {
        food, drink
    }

    public class Menu
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? ImageFile { get; set; }
        public required MenuType MenuType { get; set; }
        public required int MenuCategoryId { get; set; }
        [ForeignKey("MenuCategoryId")]
        public MenuCategory? MenuCategory { get; set; }




    }
}
