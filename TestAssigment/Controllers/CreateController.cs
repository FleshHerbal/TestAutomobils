using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAssigment.Models;

namespace TestAssigment.Controllers
{
    [Route("create")]
    public class CreateController : Controller
    {
        public CreateController(ApplicationContext context) => _context = context;

        private readonly ApplicationContext _context;

        [HttpGet]
        public IActionResult CreatePage() 
        {
            List<ViewModel> dataModel = new List<ViewModel> { };
            _context.Brands.ToList().ForEach(el => dataModel.Add(new ViewModel {Brand = el, Models = new Cars.Model[] { } }));

            return View(dataModel);
        }


        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand([FromForm(Name = "brand")] string[] brands)
        {
            List<ViewModel> dataModel = new List<ViewModel> { };
            _context.Brands.ToList().ForEach(el => dataModel.Add(new ViewModel { Brand = el, Models = new Cars.Model[] { } }));

            if (brands.Length == 0)
            {
                ViewData["message"] = "Нет данных для добавления!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
            try
            {
                foreach (string item in brands)
                {
                    if (_context.Brands.Any(d => d.Name == item || String.IsNullOrEmpty(item))) continue;

                    await _context.Brands.AddAsync(new Cars.Brand { Name = item, IsActive = true });
                }
                _context.SaveChanges();

                _context.Brands.ToList().ForEach(el => dataModel.Add(new ViewModel { Brand = el, Models = new Cars.Model[] { } }));

                ViewData["message"] = "Данные добавлены!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
            catch (Exception ex)
            {
                ViewData["message"] = "Ошибка при добавлении!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
        }

        [HttpPost("models")]
        public async Task<IActionResult> CreateModel([FromForm(Name = "model")] string[] models, [FromForm] string brandGuid)
        {
            List<ViewModel> dataModel = new List<ViewModel> { };
            _context.Brands.ToList().ForEach(el => dataModel.Add(new ViewModel { Brand = el, Models = new Cars.Model[] { } }));

            if (models.Length == 0 || String.IsNullOrEmpty(brandGuid))
            {
                ViewData["message"] = "Ошибка при добавлении моделей авто!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
            try
            {
                Cars.Brand? brand = await _context.Brands.FirstOrDefaultAsync(data => data.Guid == Guid.Parse(brandGuid));
                if (brand == null) return BadRequest();

                foreach (string item in models)
                {
                    if (_context.Models.Any(d => (d.BrandGuid == Guid.Parse(brandGuid) && d.Name == item) || String.IsNullOrEmpty(item))) continue;
                    await _context.AddAsync(new Cars.Model { Name = item, BrandGuid = brand.Guid, IsActive = true });
                }
                _context.SaveChanges();

                _context.Brands.ToList().ForEach(el => dataModel.Add(new ViewModel { Brand = el, Models = new Cars.Model[] { } }));

                ViewData["message"] = "Модели добавлены!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
            catch (Exception ex)
            {
                ViewData["message"] = "Ошибка при добавлении моделей авто!";
                return View("~/Views/Create/CreatePage.cshtml", dataModel);
            }
        }
    }
}
