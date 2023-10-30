using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var model = CommonDataService.ListOfEmployees(out rowCount, page, 10, searchValue);
            return View(model);
        }

        // Thêm nhân viên mới
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            return View();
        }

        // Chỉnh sửa thông tin nhân viên
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";
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
