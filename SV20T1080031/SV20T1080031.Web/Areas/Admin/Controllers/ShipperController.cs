using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipperController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var model = CommonDataService.ListOfShippers(out rowCount, page, 10, searchValue);
            return View(model);
        }

        // Bổ sung người giao hàng
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            return View();
        }

        // Chỉnh sửa thông tin người giao hàng
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            return View("Create");
        }

        //Xóa người giao hàng
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
