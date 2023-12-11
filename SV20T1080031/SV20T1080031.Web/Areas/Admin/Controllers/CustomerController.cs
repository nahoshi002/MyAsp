using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web.Models;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]// chuyển đến đăng nhập
    [Area ("Admin")]
    public class CustomerController : Controller
    {
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, Page_Size, searchValue ?? "");
            var model = new PaginationSearchCustomer()
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

        // Thêm khách hàng mới
        public IActionResult Create()
        {
            var model = new Customer()
            {
                CustomerID = 0
            };
            ViewBag.Title = "Bổ sung khách hàng";
            return View(model);
        }

        // Chỉnh sửa thông tin khách hàng
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCustomer(id);
            if (model == null) 
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin khách hàng";
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
            if(Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteCustomer(id);
                if (!success) 
                {
                    TempData["ErrorMessage"] = "Không được phép xóa khách hàng này";
                }
                else
                {
                    TempData["DeletedMessage"] = "Xóa khách hàng thành công!";
                }    

                return RedirectToAction("Index");
            }    
            var model = CommonDataService.GetCustomer(id);
            if(model == null)
            {
                return RedirectToAction("Index");
            }    
            return View(model);
        }

        public IActionResult Save(Customer data)
        {
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật khách hàng";

            if ( string.IsNullOrWhiteSpace(data.CustomerName))
            {
                ModelState.AddModelError(nameof(data.CustomerName), "* Tên khách hàng không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(data.ContactName))
            {
                ModelState.AddModelError(nameof(data.ContactName), "* Tên giao dịch không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(data.Address))
            {
                ModelState.AddModelError(nameof(data.Address), "* Địa chỉ không hợp lệ!");
            }
            if (string.IsNullOrWhiteSpace(data.Province))
            {
                ModelState.AddModelError(nameof(data.Province), "* Vui lòng chọn tỉnh thành!");
            }
            if (string.IsNullOrWhiteSpace(data.Phone))
            {
                ModelState.AddModelError(nameof(data.Phone), "* Số điện thoại không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(data.Email))
            {
                ModelState.AddModelError(nameof(data.Email), "* Địa chỉ email không được để trống!");
            }

            if ( !ModelState.IsValid)
            {
                return View("Create", data);
            }    



            if (data.CustomerID == 0)
            {
                int customerId = CommonDataService.AddCustomer(data);
                if(customerId > 0)
                {
                    TempData["SavedMessage"] = "Thông tin khách hàng đã được lưu lại!";
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
                bool editSuccess = CommonDataService.UpdateCustomer(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Thông tin khách hàng đã được chỉnh sửa!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Chỉnh sửa thông tin khách hàng không thành công!";
                    return View("Create", data);
                }
            }    
        }
    }
}
