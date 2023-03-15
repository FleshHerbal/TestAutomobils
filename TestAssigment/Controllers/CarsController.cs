using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using TestAssigment.Models;

namespace TestAssigment.Controllers
{
    [Route("cars")]
    public class CarsController : Controller
    {
        public CarsController(ApplicationContext context) => _context = context; 

        private readonly ApplicationContext _context;

        [HttpGet]
        public IActionResult Index()
        {
            List<ViewModel> viewModels = new List<ViewModel> { };

            // т.к в теории тут может находиться большое кол-во данных, можно реализовать пагинацию или выборку по определенному периоду.
            _context.Brands.OrderBy(data => data.Name).ToList().ForEach(async element => {
                Cars.Model[]? models = await _context.Models.Where(data => data.BrandGuid == element.Guid).ToArrayAsync(); 
                viewModels.Add(new ViewModel { Brand = element, Models = models});
            });

            return View(viewModels);
        }

        [HttpPut("edit/value")]
        public async Task<IActionResult> EditEntity([FromQuery(Name = "g")] string guid, [FromQuery(Name = "act")] string action,
            [FromQuery(Name = "v")] string value, [FromQuery(Name = "fl")] string field)
        {
            if (String.IsNullOrEmpty(guid) || String.IsNullOrEmpty(action) || String.IsNullOrEmpty(value) || String.IsNullOrEmpty(field))
                return BadRequest(new { isError = true, message = "Input data is incomplete!" });

            try
            {
                if (action == "brand")
                {
                    Cars.Brand? brand = await _context.Brands.FirstOrDefaultAsync(data => data.Guid == Guid.Parse(guid));
                    if (brand == null) return BadRequest(new { isError = true, message = "Entity is not fount!" });

                    if (field == "active") brand.IsActive = !brand.IsActive;
                    if (field == "name") brand.Name = value;
 
                    _context.Brands.Update(brand);
                }
                else if (action == "model")
                {
                    Cars.Model? model = await _context.Models.FirstOrDefaultAsync(data => data.Guid == Guid.Parse(guid));
                    if (model == null) return BadRequest(new { isError = true, message = "Entity is not fount!" });

                    if (field == "active") model.IsActive = !model.IsActive;
                    if (field == "name") model.Name = value;

                    _context.Models.Update(model);
                }

                _context.SaveChanges();
                return Ok(new { isErrpr = false, message = String.Empty });
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, new { isError = true, message = ex.Message });
            }
        }


        [HttpDelete("edit/delete")]
        public async Task<IActionResult> DeleteEntity([FromQuery(Name = "g")] string guid, [FromQuery(Name = "act")] string action)
        {
            if (String.IsNullOrEmpty(guid) || String.IsNullOrEmpty(action))
                return BadRequest(new { isError = true, message = "Input data is incomplete!" });

            if (action == "model")
            {
                try
                {
                    bool isExists = _context.Models.Any(data => data.Guid == Guid.Parse(guid));
                    if (isExists == false) return BadRequest(new { isError = true, message = "Entity is not found!" });

                    _context.Remove(await _context.Models.FirstOrDefaultAsync(e => e.Guid == Guid.Parse(guid)));

                    _context.SaveChanges();

                    return Ok(new { isError = false, message = String.Empty });
                }
                catch (Exception ex)
                {
                    return StatusCode((int) HttpStatusCode.InternalServerError, new { isError = true, message = ex.Message });
                }
            }
            else 
            {
                try
                {
                    Cars.Brand? brand = _context.Brands.FirstOrDefault(data => data.Guid == Guid.Parse(guid));
                    if (brand == null) return BadRequest(new { isError = true, message = "Entity is not found!" });

                    Action actionF = new(async () => {
                        try
                        {
                            using ApplicationContext appContext = new();

                            Cars.Model[] models = await appContext.Models.Where(data => data.BrandGuid == brand.Guid).ToArrayAsync();

                            appContext.Models.RemoveRange(models);
                            await appContext.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    });
                    Task.Run(actionF);

                    _context.Brands.Remove(brand);
                    _context.SaveChanges();

                    return Ok(new { isError = false, message = String.Empty });
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { isError = true, message = ex.Message });
                }
            }
        }
    }
}
