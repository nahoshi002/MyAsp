using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV20T1080031.DomainModels;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace SV20T1080031.DataLayers.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        public SupplierDAL(string connectionString) : base(connectionString)
        {
        }

        public int add(Supplier data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*) from Suppliers
                            where (@searchValue = N'') or (SupplierName like @searchValue)";
                var parameters = new { searchValue = searchValue };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }

        public bool Delete(Supplier data)
        {
            throw new NotImplementedException();
        }

        public Supplier? get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUser(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Supplier> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Supplier> data;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                        (
	                        select *,
	                        ROW_NUMBER() over (order by SupplierName) as RowNumber
	                        from Suppliers
	                        where (@searchValue = N'') or (SupplierName like @searchValue)
                        )

                        select * from cte
                        where (@pageSize = 0)
	                        or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
	                        order by RowNumber";
                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue
                };
                data = connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }
            return data;
        }

        public bool Update(Supplier data)
        {
            throw new NotImplementedException();
        }
    }
}
