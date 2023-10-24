using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area ("Admin")]
    public class CustomerController : Controller
    {
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var model = CommonDataService.ListOfCustomers(out rowCount, page, 10, searchValue);
            return View(model);
        }

        // Thêm khách hàng mới
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            return View();
        }

        // Chỉnh sửa thông tin khách hàng
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View("Create");
        }

        //Đổi mật khẩu
        public IActionResult ChangePass(int id = 0)
        {
            return View();
        }

        //Xóa khách hàng
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
