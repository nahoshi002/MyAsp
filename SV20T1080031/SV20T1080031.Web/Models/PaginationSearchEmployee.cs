using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    public class PaginationSearchEmployee : PaginationSearchBaseResult
    {
        public IList<Employee> Data { get; set; }
    }
}
