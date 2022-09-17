namespace FoodStore.Models.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            Product = new();
        }

        public  Product Product { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
