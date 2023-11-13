using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    public class PaginationSearchShipper : PaginationSearchBaseResult
    {
        public IList<Shipper> Data { get; set; }
    }
}
