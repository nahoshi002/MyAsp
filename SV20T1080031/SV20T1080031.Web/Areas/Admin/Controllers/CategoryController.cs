using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web.Models;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, Page_Size, searchValue ?? "");
            var model = new PaginationSearchCategory()
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
