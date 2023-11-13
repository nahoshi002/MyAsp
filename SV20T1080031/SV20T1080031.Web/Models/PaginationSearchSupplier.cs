using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    public class PaginationSearchSupplier : PaginationSearchBaseResult
    {
        public IList<Supplier> Data { get; set; }
    }
}
