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
        /// <summary>
        /// 
        /// </summary>
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string conectionString = "server=DESKTOP-C4C4J90\\ATEODINANG;integrated security=true;user=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";
            customerDB = new DataLayers.SQLServer.CustomerDAL(conectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(conectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(conectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(conectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(conectionString);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount,
                                                    int page = 1,
                                                    int pageSize = 0,
                                                    string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount,
                                                    int page = 1,
                                                    int pageSize = 0,
                                                    string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount,
                                                    int page = 1,
                                                    int pageSize = 0,
                                                    string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(out int rowCount,
                                                    int page = 1,
                                                    int pageSize = 0,
                                                    string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
    }
}
