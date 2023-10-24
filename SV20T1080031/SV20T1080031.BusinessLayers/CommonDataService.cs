using SV20T1080031.DataLayers;
using SV20T1080031.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.BusinessLayers
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Customer> customerDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string conectionString = "server=DESKTOP-C4C4J90\\ATEODINANG;integrated security=true;user=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";
            customerDB = new DataLayers.SQLServer.CustomerDAL(conectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, 
                                                    int page = 1, 
                                                    int pageSize = 0, 
                                                    string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
    }
}
