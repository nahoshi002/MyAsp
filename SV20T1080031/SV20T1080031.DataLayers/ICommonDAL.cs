using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.DataLayers
{
    /// <summary>
    /// Mô tả chung cho các trang nhân viên, người giao hàng, khách hàng, nhà cung cấp, loại hàng.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T> where T : class  //generic
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách các dữ liệu dưới dạng phân trang.
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (bằng 0 nếu không tiến hành phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue = ""); // Lấy dữ liệu theo kiểu đồng bộ

        /// <summary>
        /// Đếm số dòng dữ liệu thỏa điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm (chuỗi rỗng nếu lấy toàn bộ dữ liệu)</param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Bổ sung thêm dữ liệu vào database. Hàm trả về ID của dữ liệu
        /// được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// Lấy bản ghi dựa vào ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// Kiểm tra xem dữ liệu có mã ID hiện có đang được sử dụng bởi các dữ liệu khác hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
