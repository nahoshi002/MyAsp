using Microsoft.AspNetCore.Mvc;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    public class SupplierController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        // Tạo nhà cung cấp
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View();
        }

        // Chỉnh sửa thông tin nhà cung cấp
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View("Create");
        }

        //Xóa nhà cung cấp
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
