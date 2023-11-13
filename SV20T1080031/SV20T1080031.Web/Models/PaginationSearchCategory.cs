using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    public class PaginationSearchCategory : PaginationSearchBaseResult
    {
        public IList<Category> Data { get; set; }
    }
}
