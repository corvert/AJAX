using Microsoft.AspNetCore.Mvc;

namespace AJAX.Controllers
{
    public class CityController : Controller
    {
      private readonly AppDBContext _context;

        public CityController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           List<City> cities = _context.Cities.ToList();
            return View(cities);
        }




        [HttpGet]
        public IActionResult Create()
        {
           City city = new City();
            ViewBag.Countries = GetCountries();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(City city)
        {
           
                _context.Add(city);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
         
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            City City = _context.Cities.Where(c => c.Id == id).FirstOrDefault();

            return View(City);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            City City = _context.Cities.Where(c => c.Id == Id).FirstOrDefault();
            ViewBag.Countries = GetCountries();
            return View(City);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(City city)
        {
           
            _context.Attach(city);
            _context.Entry(city).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            City city = _context.Cities.Where(c => c.Id == id).FirstOrDefault();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(City city)
        {
            _context.Attach(city);
            _context.Entry(city).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetCountries()
        {
            var lstCountries = new List<SelectListItem>();
            List<Country> countries = _context.Countries.ToList();
            lstCountries = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var defitem = new SelectListItem()
            {
                Value = "",
                Text = "Select Country"
            };
            lstCountries.Insert(0, defitem);
            return lstCountries;
        }
    }
}
