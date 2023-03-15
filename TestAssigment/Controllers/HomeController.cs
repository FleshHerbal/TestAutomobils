using Microsoft.AspNetCore.Mvc;
using TestAssigment.Models;

namespace TestAssigment.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ApplicationContext context) => _context = context;

        private readonly ApplicationContext _context;
        [Route("/")]
        public IActionResult Index()
        {
            if (_context.Brands.Count() == 0) FirstData.CreateData(_context);

            return Redirect("/cars");
        }
    }
    internal static class FirstData
    {
        public static void CreateData(ApplicationContext context)
        {
            List<DataModel> dataArray = new List<DataModel>() {
                new DataModel { BrandName = "Nissan", Models = new string[] { "Tiida", "Almera", "Juke", "Skyline", "March", "x-Trail"} },
                new DataModel { BrandName = "Lada", Models = new string[] { "Granta", "Priora", "Kalina", "VAR2101", "Vesta", "x-Ray" } },
                new DataModel { BrandName = "Toyota", Models = new string[] { "Crown", "Camry", "Corolla", "MarkII", "Prius", "Supra" } },
                new DataModel { BrandName = "Ford", Models = new string[] {"Focus", "Mustang", "Fusion", "Freda", "Freestyle", "Mondeo" } },
                new DataModel { BrandName = "Daewoo", Models = new string[] { "Nexia", "Matiz", "Leganza", "Tacuma", "Nubira", "Veritaz" } },
                new DataModel { BrandName = "chevrolet", Models = new string[] { "Aveo", "Cruze", "Corvette", "Spark", "Optra", "Onix" } }
            };

            foreach (DataModel item in dataArray)
            {
                Cars.Brand newBrand = new() { Name = item.BrandName, IsActive = new Random().Next(0, 99) <= 49 };
                context.Brands.Add(newBrand);

                foreach (string itemModel in item.Models)
                {
                    context.Models.Add(new Cars.Model { Name = itemModel, BrandGuid = newBrand.Guid, IsActive = new Random().Next(0, 99) <= 49 });
                }
            }
            context.SaveChanges();
        }
        private class DataModel
        {
            public string BrandName { get; set; }
            public string[] Models { get; set; }
        }
    }
}
