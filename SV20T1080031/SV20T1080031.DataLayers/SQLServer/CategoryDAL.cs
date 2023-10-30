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
    /// <summary>
    /// 
    /// </summary>
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int add(Category data)
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
                var sql = @"select count(*) from Categories
                            where (@searchValue = N'') or (CategoryName like @searchValue)";
                var parameters = new { searchValue = searchValue };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }

        public bool Delete(Category data)
        {
            throw new NotImplementedException();
        }

        public Category? get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUser(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                        (
	                        select *,
	                        ROW_NUMBER() over (order by CategoryName) as RowNumber
	                        from Categories
	                        where (@searchValue = N'') or (CategoryName like @searchValue)
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
                data = connection.Query<Category>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();

                connection.Close();
            }
            return data;
        }

        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
