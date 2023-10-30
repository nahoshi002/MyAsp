using Dapper;
using SV20T1080031.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1080031.DataLayers.SQLServer
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int add(Employee data)
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
                var sql = @"select count(*) from Employees
                            where (@searchValue = N'') or (FullName like @searchValue)";
                var parameters = new { searchValue = searchValue };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }

        public bool Delete(Employee data)
        {
            throw new NotImplementedException();
        }

        public Employee? get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUser(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                        (
	                        select EmployeeId, FullName, BirthDate, Address, Phone, Email, Photo, IsWorking,
	                        ROW_NUMBER() over (order by FullName) as RowNumber
	                        from Employees
	                        where (@searchValue = N'') or (FullName like @searchValue)
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
                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            throw new NotImplementedException();
        }
    }
}
