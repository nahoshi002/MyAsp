﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1080031.BusinessLayer;
using SV20T1080031.BusinessLayers;
using SV20T1080031.DomainModels;
using SV20T1080031.Web;
using SV20T1080031.Web.AppCodes;
using SV20T1080031.Web.Models;
using System.Drawing.Printing;
using System.Reflection;

namespace LiteCommerce.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]// chuyển đến đăng nhập
    /// <summary>
    /// 
    /// </summary>
    [Area("Admin")]
    public class OrderController : Controller
    {
        private const string Product_Search = "Product_Search";
        private const string Shopping_Cart = "Shopping_Cart";
        private const string ERROR_MESSAGE = "ErrorMessage";
        private const string SESSION_CONDITION = "OrderCondition";
        public const int Page_Size = 10; // Tạo một biến hằng để đồng bộ thuộc tính cho trang web.
        /// <summary>
        /// Hiển thị danh sách đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var input = ApplicationContext.GetSessionData<PaginationSearchOrderInput>(SESSION_CONDITION);
            if (input == null)
            {
                input = new PaginationSearchOrderInput()
                {
                    Page = 1,
                    PageSize = Page_Size,
                    SearchValue = "",
                    Status = 0,
                };
            }

            return View(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult Search(PaginationSearchOrderInput input)
        {
            int rowCount = 0;
            var data = OrderDataService.ListOrders(input.Page, input.PageSize, input.Status, input.SearchValue, out rowCount);
            var model = new PaginationSearchOrder()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };
            ApplicationContext.SetSessionData(SESSION_CONDITION, input);//lưu lại điều kiện tìm kiếm

            return View(model);
        }

        /// <summary>
        /// Giao diện trang tạo đơn hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Giao diện trang chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id = 0)
        {
            return View();
        }
        /// <summary>
        /// Giao diện cập nhật thông tin chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>        
        [HttpGet]
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {         
            return View();
        }
        /// <summary>
        /// Cập nhật chi tiết đơn hàng (trong giỏ hàng)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        [HttpPost]        
        public IActionResult UpdateDetail(int id = 0, int productId = 0, int quantity = 0, decimal salePrice = 0)
        {
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productID"></param>
        /// <returns></returns>        
        public IActionResult DeleteDetail(int id = 0, int productID = 0)
        {            
            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id = 0)
        {
            //TODO: Code chức năng để xóa đơn hàng (nếu được phép xóa)

            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            //TODO: Duyệt chấp nhận đơn hàng

            return RedirectToAction("Details", new { id = id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Shipping(int id = 0, int shipperID = 0)
        {            
            if (Request.Method == "GET")
                return View();
            else
            {
                //TODO: Chuyển đơn hàng cho người giao hàng

                return RedirectToAction("Details", new { id = id });
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            //TODO: Ghi nhận hoàn tất đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }
        /// <summary>
        /// Hủy bỏ đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            //TODO: Hủy đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }
        /// <summary>
        /// Từ chối đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            //TODO: Từ chối đơn hàng

            return RedirectToAction($"Details", new { id = id });
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public IActionResult SearchProducts()
        {
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Bổ sung thêm hàng vào giỏ hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart()
        {            
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa 1 mặt hàng khỏi giỏ hàng
        /// </summary>        
        /// <returns></returns>
        public ActionResult RemoveFromCart()
        {            
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Xóa toàn bộ dữ liệu trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearCart()
        {            
            return RedirectToAction("Create");
        }
        /// <summary>
        /// Khởi tạo đơn hàng và chuyển đến trang Details sau khi khởi tạo xong để tiếp tục quá trình xử lý đơn hàng
        /// </summary>        
        /// <returns></returns>
        [HttpPost]
        public ActionResult Init()
        {
            int orderId = 111;

            //TODO: Khởi tạo đơn hàng và nhận mã đơn hàng được khởi tạo

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
