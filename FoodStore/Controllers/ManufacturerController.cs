using FoodStore.Data;
using FoodStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ManufacturerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Manufacturer> manList = _db.Manufacturer;
            return View(manList);
        }
    }
}
