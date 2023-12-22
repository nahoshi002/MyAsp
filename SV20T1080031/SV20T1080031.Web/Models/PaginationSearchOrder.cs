using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    /// <summary>
    /// Danh sách dữ liệu Order
    /// </summary>
    public class PaginationSearchOrder :PaginationSearchBaseResult
    {
        /// <summary>
        /// Dữ liệu đơn hàng
        /// </summary>
        public List<Order> Data { get; set; }
    }
}
