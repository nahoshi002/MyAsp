using SV20T1080031.DataLayers;
using SV20T1080031.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV20T1080031.DataLayers.SQLServer;

namespace SV20T1080031.BusinessLayers
{
    public static class UserAccountService
    {
        static readonly IUserAccountDAL employeeUserAccountDB;

        static UserAccountService()
        {
            string connectionString = "server=DESKTOP-C4C4J90\\ATEODINANG;integrated security=true;user=sa;password=123;database=LiteCommerceDB;TrustServerCertificate=true";

            employeeUserAccountDB = new DataLayers.SQLServer.EmployeeUserAccountDAL(connectionString);
        }

        public static UserAccount? Authorize(string username, string password, TypeOfAccount typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case TypeOfAccount.Employee:
                    return employeeUserAccountDB.Authorize(username, password);
                default: return null;
            }
        }

       
        public static bool ChangePassword(string username, string password, TypeOfAccount typeOfAccount)
        {
            switch (typeOfAccount)
            {
                case TypeOfAccount.Employee:
                    return employeeUserAccountDB.ChangePassword(username, password);
                default: return false;
            }
        }
    }
    

    public enum TypeOfAccount
    {
        Employee,
        Customer
    }
}