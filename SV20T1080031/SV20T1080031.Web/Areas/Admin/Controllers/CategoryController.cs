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
    public class CategoryController : Controller
    {
        private const string Category_Search = "Category_Search";
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchInput>(Category_Search);
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
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new PaginationSearchCategory()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(Category_Search, input);//lưu lại điều kiện tìm kiếm

            string errorMessage = Convert.ToString(TempData["ErrorMessage"]);
            ViewBag.ErrorMessage = errorMessage;
            string deletedMessage = Convert.ToString(TempData["DeletedMessage"]);
            ViewBag.DeletedMessage = deletedMessage;
            string savedMessage = Convert.ToString(TempData["SavedMessage"]);
            ViewBag.SavedMessage = savedMessage;

            return View(model);
        }

        // Tạo loại hàng
        public IActionResult Create()
        {
            var model = new Category()
            {
                CategoryID = 0
            };
            ViewBag.Title = "Bổ sung loại hàng";
            return View(model);
        }

        // Chỉnh sửa thông tin loại hàng
        public IActionResult Edit(int id = 0)
        {
            var model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Cập nhật loại hàng";
            return View("Create",model);
        }

        //Xóa loại hàng
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool success = CommonDataService.DeleteCategory(id);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Không được phép xóa loại hàng này";
                }
                else
                {
                    TempData["DeletedMessage"] = "Xóa loại hàng thành công!";
                }

                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Lưu thông tin loại hàng
        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật loại hàng";

            if (string.IsNullOrWhiteSpace(data.CategoryName))
            {
                ModelState.AddModelError(nameof(data.CategoryName), "* Tên loại hàng không được để trống!");
            }
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                ModelState.AddModelError(nameof(data.Description), "* Vui lòng nhập mô tả loại hàng!");
            }

            if (!ModelState.IsValid)
            {
                return View("Create", data);
            }

            if (data.CategoryID == 0)
            {
                int categoryId = CommonDataService.AddCategory(data);
                if (categoryId > 0)
                {
                    TempData["SavedMessage"] = "Thông tin loại hàng đã được lưu lại!";
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
                bool editSuccess = CommonDataService.UpdateCategory(data);
                if (editSuccess)
                {
                    TempData["SavedMessage"] = "Thông tin loại hàng đã được chỉnh sửa!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Chỉnh sửa thông tin loại hàng không thành công!";
                    return View("Create", data);
                }
            }
        }
    }
}
