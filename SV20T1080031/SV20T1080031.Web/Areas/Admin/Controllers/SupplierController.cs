using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web.AppCodes;
using SV20T1080031.Web.Models;
using System.Drawing.Printing;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]// chuyển đến đăng nhập
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private const string Supplier_Search = "Supplier_Search";
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(Supplier_Search);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = Page_Size,
                    SearchValue = ""
                };
            }

            return View(input);
        }

        /// <summary>
        /// Hàm trả về danh sách tìm kiếm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IActionResult Search(PaginationSearchInput input)
        {

            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new PaginationSearchSupplier()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(Supplier_Search, input);//lưu lại điều kiện tìm kiếm

            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            string deletedMessage = Convert.ToString(TempData["DeletedMessage"]);
            ViewBag.DeletedMessage = deletedMessage;
            string savedMessage = Convert.ToString(TempData["SavedMessage"]);
            ViewBag.SavedMessage = savedMessage;

            return View(model);
        }

        // Tạo nhà cung cấp
        public IActionResult Create()
        {
            var model = new Supplier()
            {
                SupplierID = 0
            };
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View(model);
        }

        // Chỉnh sửa thông tin nhà cung cấp
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View("Create", model);
        }

        //Xóa nhà cung cấp
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteSupplier(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Không được phép xóa nhà cung cấp này";
                }
                else
                {
                    TempData["DeletedMessage"] = "Xóa nhà cung cấp thành công!";
                }

                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetSupplier(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Save(Supplier data)
        {
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật thông tin nhà cung cấp";
            if (string.IsNullOrWhiteSpace(data.SupplierName))
            {
                ModelState.AddModelError(nameof(data.SupplierName), "* Tên nhà cung cấp không được để trống!");
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

            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.SupplierID == 0)
            {
                int supplierId = CommonDataService.AddSupplier(data);
                if (supplierId > 0)
                {
                    TempData["SavedMessage"] = "Thông tin nhà cung cấp đã được lưu lại!";
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
                bool editSuccess = CommonDataService.UpdateSupplier(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Thông tin nhà cung cấp đã được chỉnh sửa!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Chỉnh sửa thông tin nhà cung cấp không thành công!";
                    return View("Create", data);
                }
            }
        }
    }
}
