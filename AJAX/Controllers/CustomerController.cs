using Microsoft.AspNetCore.Mvc;

namespace AJAX.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _webHost;
        public CustomerController(AppDBContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
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

            string uniqueFileName = GetProfilePhotoFileName(customer);
            customer.PhotoUrl = uniqueFileName;

            _context.Add(customer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Customer customer = _context.Customers
                .Include(co => co.City)
                .Include(co => co.City.Country)
                .Where(c => c.Id == id).FirstOrDefault();



            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer customer = _context.Customers.Include(co => co.City)
                .Where(c => c.Id == id).FirstOrDefault();

            customer.CountryId = customer.City.CountryId;

            ViewBag.Countries = GetCountries();
            ViewBag.Cities = GetCities(customer.CountryId);
            return View(customer);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {

            if (customer.ProfilePhoto != null)
            {
                string uniqueFileName = GetProfilePhotoFileName(customer);
                customer.PhotoUrl = uniqueFileName;
            }
         

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
        public JsonResult GetCitiesByCountry(int countryId)
        {
            List<SelectListItem> cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();
            return Json(cities);
        }

        private string GetProfilePhotoFileName(Customer customer)
        {
            string uniqueFileName = null;
            if(customer.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + customer.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customer.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private List<SelectListItem> GetCities(int countryId)
        {
          List<SelectListItem> cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(n => n.Name)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name
                }).ToList();
            return cities;
        }
    }
}
