using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web.Models;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, page, Page_Size, searchValue ?? "");
            var model = new PaginationSearchEmployee()
            {
                Page = page,
                PageSize = Page_Size,
                SearchValue = searchValue ?? "", // Nếu giá trị của searchValue là null thì giá trị của nó là một chuỗi rỗng
                RowCount = rowCount,
                Data = data
            };

            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            string deletedMessage = Convert.ToString(TempData["DeletedMessage"]);
            ViewBag.DeletedMessage = deletedMessage;
            string savedMessage = Convert.ToString(TempData["SavedMessage"]);
            ViewBag.SavedMessage = savedMessage;

            return View(model);
        }

        // Thêm nhân viên mới
        public IActionResult Create()
        {
            var model = new Employee()
            {
                EmployeeID = 0
            };
            ViewBag.Title = "Bổ sung nhân viên";
            return View(model);
        }

        // Chỉnh sửa thông tin nhân viên
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin nhân viên";
            return View("Create", model);
        }

        //Đổi mật khẩu
        public IActionResult ChangePass(int id = 0)
        {
            return View();
        }

        //Xóa khách hàng
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteEmployee(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Không được phép xóa nhân viên này";
                }
                else
                {
                    TempData["DeletedMessage"] = "Xóa nhân viên thành công!";
                }

                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Save(Employee data)
        {
            ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";

            if (data.EmployeeID == 0)
            {
                int employeeId = CommonDataService.AddEmployee(data);
                if (employeeId > 0)
                {
                    TempData["SavedMessage"] = "Thông tin nhân viên đã được lưu lại!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Không bổ sung được dữ liệu!";
                    return View("Create", data);
                }
            }
            else
            {
                bool editSuccess = CommonDataService.UpdateEmployee(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Thông tin nhân viên đã được chỉnh sửa!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Chỉnh sửa thông tin nhân viên không thành công!";
                    return View("Create", data);
                }
            }
        }
    }
}
