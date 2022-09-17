using FoodStore.Data;
using FoodStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodStore.Controllers
{
    public class CategoryController : Controller
    {
        // Переменная контекста для доступа к базе данных
        private readonly ApplicationDbContext _db;

        // Внедряем зависимость ApplicationDbContext через конструктор
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Создаем список записей категорий из базы данных
            IEnumerable<Category> categoryList = _db.Category;
            // Возвращаем загруженный список в представление
            return View(categoryList);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);

        }


        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cat = _db.Category.Find(id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category cat)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(cat);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cat);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cat = _db.Category.Find(id);
            if (cat == null)
            {
                return NotFound();
            }

            return View(cat);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var cat = _db.Category.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            _db.Category.Remove(cat);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}
