using Microsoft.AspNetCore.Mvc;

namespace AJAX.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AppDBContext _context;
        public CustomerController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Customer> customers = _context.Customers.ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
         Customer customer = new Customer();
         ViewBag.Countries = GetCountries();
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
           _context.Add(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Customer customer = _context.Customers.Where(c => c.Id == id).FirstOrDefault();
           
            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer customer = _context.Customers.Where(c => c.Id == id).FirstOrDefault();
            ViewBag.Countries = GetCountries();
            return View(customer);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
           
                _context.Attach(customer);
            _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            
      
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Customer customer = _context.Customers.Where(c => c.Id == id).FirstOrDefault();
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = EntityState.Deleted;
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

        [HttpGet]
        public JsonResult GetCitiesByCountry(int countyId)
        {
            List<SelectListItem> cities = _context.Cities
                .Where(c => c.CountryId == countyId)
                .OrderBy(n => n.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            return Json(cities);
        }
    }
}
