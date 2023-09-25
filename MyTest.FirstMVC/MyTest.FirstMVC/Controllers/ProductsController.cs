using Microsoft.AspNetCore.Mvc;

namespace MyTest.FirstMVC.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            List<Models.Person> data = new List<Models.Person>();
            data.Add(new Models.Person() { Id = 1, Name = "Ateo", Age = "21" });
            data.Add(new Models.Person() { Id = 2, Name = "AteoDiNang", Age = "21" });

            return View(data);
        }
        
        public string Hello(string id, string subid)
        {
            return $"hello {id} {subid}";
        }
    }
}
