using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web.AppCodes;
using SV20T1080031.Web.Models;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]// chuyển đến đăng nhập
    [Area("Admin")]
    public class ShipperController : Controller
    {
        private const string Shipper_Search = "Shipper_Search";
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(Shipper_Search);
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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new PaginationSearchShipper()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(Shipper_Search, input);//lưu lại điều kiện tìm kiếm

            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            string deletedMessage = Convert.ToString(TempData["DeletedMessage"]);
            ViewBag.DeletedMessage = deletedMessage;
            string savedMessage = Convert.ToString(TempData["SavedMessage"]);
            ViewBag.SavedMessage = savedMessage;

            return View(model);
        }

        // Bổ sung người giao hàng
        public IActionResult Create()
        {
            var model = new Shipper()
            {
                ShipperID = 0
            };
            ViewBag.Title = "Bổ sung người giao hàng";
            return View(model);
        }

        // Chỉnh sửa thông tin người giao hàng
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            return View("Create",model);
        }

        //Xóa người giao hàng
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteShipper(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Không được phép xóa người giao hàng này";
                }
                else
                {
                    TempData["DeletedMessage"] = "Xóa người giao hàng thành công!";
                }

                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Lưu thông tin người giao hàng
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật người giao hàng";

            if (string.IsNullOrWhiteSpace(data.ShipperName))
            {
                ModelState.AddModelError(nameof(data.ShipperName), "* Vui lòng nhập tên người giao hàng!");
            }
            if (string.IsNullOrWhiteSpace(data.Phone))
            {
                ModelState.AddModelError(nameof(data.Phone), "* Vui lòng nhập số điện thoại!");
            }

            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.ShipperID == 0)
            {
                int shipperId = CommonDataService.AddShipper(data);
                if (shipperId > 0)
                {
                    TempData["SavedMessage"] = "Thông tin người giao hàng đã được lưu lại!";
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
                bool editSuccess = CommonDataService.UpdateShipper(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Thông tin người giao hàng đã được chỉnh sửa!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Chỉnh sửa thông tin người giao hàng không thành công!";
                    return View("Create", data);
                }
            }
        }
    }
}
