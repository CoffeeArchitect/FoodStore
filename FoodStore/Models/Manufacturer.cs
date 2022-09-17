using System.ComponentModel.DataAnnotations;

namespace FoodStore.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
