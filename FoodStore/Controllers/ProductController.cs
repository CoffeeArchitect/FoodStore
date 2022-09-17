using FoodStore.Data;
using FoodStore.Models;
using FoodStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Controllers
{

    public class ProductController : Controller
    {
        // Переменная контекста
        private readonly ApplicationDbContext _db;

        // Переменная для хранения рабочего окружения
        private readonly IWebHostEnvironment _webHostEnvironment;
        // Внедрение зависимости создания контекста через конструктор
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> productList = _db.Product.Include(u => u.Category).Include(u => u.Manufacturer);

            return View(productList);
        }


        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            // Список для выбора категории
            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(item => new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                }),
                ManufacturerSelectList = _db.Manufacturer.Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                }),
            };


            if (id == null)
            {
                return View(viewModel);
            }
            else
            {
                // Обновляем выбранную позицию товара
                viewModel.Product = _db.Product.Find(id);
                if (viewModel.Product == null)
                {
                    return NotFound();
                }
                return View(viewModel);
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Переменная для хранения файла изображения
                var files = HttpContext.Request.Form.Files;
                // Переменная для указания пути каталога
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (viewModel.Product.Id == 0)
                {
                    // Генерация имени файла изображения
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    // Сохраниения файла в каталоге проекта
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    viewModel.Product.Image = fileName + extension;

                    _db.Product.Add(viewModel.Product);
                }
                else
                {
                    // В случае если объект существует обновляем его
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == viewModel.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        viewModel.Product.Image = fileName + extension;
                    }
                    else
                    {
                        viewModel.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(viewModel.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            viewModel.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Title,
                Value = i.Id.ToString()
            });
            viewModel.ManufacturerSelectList = _db.Manufacturer.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            
            return View(viewModel);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category)
                .Include(u => u.Manufacturer)
                .FirstOrDefault(u => u.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }


            _db.Product.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }
    }
}
