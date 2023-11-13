using SV20T1080031.DomainModels;

namespace SV20T1080031.Web.Models
{
    public class PaginationSearchCustomer : PaginationSearchBaseResult
    {
        public IList<Customer> Data { get; set; }
    }
}
