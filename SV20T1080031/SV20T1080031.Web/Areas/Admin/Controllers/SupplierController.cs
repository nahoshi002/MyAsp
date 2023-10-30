using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;

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
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var model = CommonDataService.ListOfSuppliers(out rowCount, page, 10, searchValue);
            return View(model);
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
