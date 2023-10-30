﻿using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayers;

namespace SV20T1080031.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var model = CommonDataService.ListOfCategories(out rowCount, page, 10, searchValue);
            return View(model);
        }

        // Tạo loại hàng
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            return View();
        }

        // Chỉnh sửa thông tin loại hàng
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật loại hàng";
            return View("Create");
        }

        //Xóa loại hàng
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
    }
}
