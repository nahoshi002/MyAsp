using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
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
    public class OrderDAL : _BaseDAL, IOrderDAL
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Chuyển đổi dữ liệu để đọc dữ liệu từ sql sang Order
        /// </summary>
        /// <param name="dbReader"></param>
        /// <returns></returns>
        private Order DataReaderToOrder(SqlDataReader dbReader)
        {
            return new Order()
            {
                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                OrderTime = Convert.ToDateTime(dbReader["OrderTime"]),
                AcceptTime = dbReader["AcceptTime"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dbReader["AcceptTime"]),
                ShippedTime = dbReader["ShippedTime"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dbReader["ShippedTime"]),
                FinishedTime = dbReader["FinishedTime"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dbReader["FinishedTime"]),
                Status = Convert.ToInt32(dbReader["Status"]),
                CustomerID = dbReader["CustomerID"] is DBNull ? (int?)null : Convert.ToInt32(dbReader["CustomerID"]),
                CustomerName = dbReader["CustomerName"].ToString(),
                CustomerContactName = dbReader["CustomerContactName"].ToString(),
                CustomerAddress = dbReader["CustomerAddress"].ToString(),
                CustomerEmail = dbReader["CustomerEmail"].ToString(),

                EmployeeID = dbReader["EmployeeID"] is DBNull ? (int?)null : Convert.ToInt32(dbReader["EmployeeID"]),
                EmployeeFullName = dbReader["EmployeeFullName"].ToString(),

                ShipperID = dbReader["ShipperID"] is DBNull ? (int?)null : Convert.ToInt32(dbReader["ShipperID"]),
                ShipperName = dbReader["ShipperName"].ToString(),
                ShipperPhone = dbReader["ShipperPhone"].ToString()
            };
        }

        /// <summary>
        /// Chuyển đổi dữ liệu để đọc dữ liệu từ sql sang OrderDetail
        /// </summary>
        /// <param name="dbReader"></param>
        /// <returns></returns>
        private OrderDetail DataReaderToOrderDetail(SqlDataReader dbReader)
        {
            return new OrderDetail()
            {
                OrderDetailID = Convert.ToInt32(dbReader["OrderDetailID"]),
                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                ProductName = Convert.ToString(dbReader["ProductName"]),
                Unit = Convert.ToString(dbReader["Unit"]),
                Photo = Convert.ToString(dbReader["Photo"]),
                Quantity = Convert.ToInt32(dbReader["Quantity"]),
                SalePrice = Convert.ToDecimal(dbReader["SalePrice"])
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(Order data, IEnumerable<OrderDetail> details)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                //Tạo mới đơn hàng
                var sqlAddOrder = @"	if exists(select * from Products where Productid =  @Productid)
                         select -1
                     else
                         begin
                            INSERT INTO Orders(CustomerID, OrderTime, EmployeeID, AcceptTime, ShipperID, ShippedTime, FinishedTime, Status)
                                         VALUES(@customerID, @orderTime, @employeeID, @acceptTime, @shipperID, @shippedTime, @finishedTime, @status);
                                         SELECT @@identity;
                         end";
                var orderParameters = new
                {
                    customerID = data.CustomerID,
                    orderTime = data.OrderTime,
                    employeeID = data.EmployeeID,
                    acceptTime = data.AcceptTime,
                    shipperID = data.ShipperID,
                    shippedTime = data.ShippedTime,
                    finishedTime = data.FinishedTime,
                    status = data.Status,
                };
                id = connection.ExecuteScalar<int>(sql: sqlAddOrder, param: orderParameters, commandType: CommandType.Text);

                //Thêm các mặt hàng vào OrderDetail có OrderID = id
                var sqlAddOrderDetail = @"	if exists(select * from Products where Productid =  @Productid)
                         select -1
                     else
                         begin
                            INSERT INTO OrderDetails(OrderID, ProductID, Quantity, SalePrice) 
                                         VALUES(@orderID, @productID, @quantity, @salePrice);
                         end";

                foreach (var item in details)
                {
                    var orderDetailsparameters = new
                    {
                        orderID = id,
                        productID = item.ProductID,
                        quantity = item.Quantity,
                        salePrice = item.SalePrice,
                    };
                    connection.Execute(sqlAddOrderDetail, orderDetailsparameters);
                }
                connection.Close();
            };
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Count(int status = 0, string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"SELECT  COUNT(*)
                                    FROM    Orders as o
                                            LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID
                                            LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID
                                            LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID
                                    WHERE   (@status = 0 OR o.Status = @status)
                                        AND (@searchValue = N'' OR c.CustomerName LIKE @searchValue OR s.ShipperName LIKE @searchValue)";

                var parameters = new
                {
                    searchValue = searchValue,
                    Status = status,
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Delete(int orderID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM OrderDetails WHERE OrderID = @orderID;                                    " +
                            "DELETE FROM Orders WHERE OrderID = @orderID;";
                var parameters = new { orderID = orderID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool DeleteDetail(int orderID, int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM OrderDetails WHERE OrderID = @orderID AND ProductID = @productID";
                var parameters = new
                {
                    orderID = orderID,
                    productID = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Order Get(int orderID)
        {
            Order? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT  o.*," +
                    "c.CustomerName," +
                    "c.ContactName as CustomerContactName," +
                    "c.Address as CustomerAddress, " +
                    "c.Email as CustomerEmail," +
                    "e.FullName as EmployeeFullName," +
                    "s.ShipperName," +
                    "s.Phone as ShipperPhone" +
                    "FROM Orders as o" +
                    "LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID" +
                    "LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID" +
                    "LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID" +
                    "WHERE o.OrderID = @OrderID";
                var parameters = new
                {
                    orderID = orderID
                };
                using (var cmd = new SqlCommand(sql, (SqlConnection)connection))
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters, null) ?? DBNull.Value);
                    }

                    connection.Open();
                    using (var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dbReader.Read())
                        {
                            data = DataReaderToOrder(dbReader);
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public OrderDetail GetDetail(int orderID, int productID)
        {
            OrderDetail? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT	od.*, p.ProductName, p.Unit, p.Photo		
                                    FROM	OrderDetails AS od
		                                    JOIN Products AS p ON od.ProductID = p.ProductID
                                    WHERE	od.OrderID = @orderID AND od.ProductID = @productID";
                var parameters = new
                {
                    orderID = orderID,
                    productID = productID
                };
                using (var cmd = new SqlCommand(sql, (SqlConnection)connection))
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters, null) ?? DBNull.Value);
                    }

                    connection.Open();
                    using (var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (dbReader.Read())
                        {
                            data = DataReaderToOrderDetail(dbReader);
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, string searchValue = "")
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (var connection = OpenConnection())
            {
                var sql = @"SELECT  *
                    FROM    (
                            SELECT  o.*,
                                    c.CustomerName,
                                    c.ContactName as CustomerContactName,
                                    c.Address as CustomerAddress,
                                    c.Email as CustomerEmail,
                                    e.FullName as EmployeeFullName,
                                    s.ShipperName,
                                    s.Phone as ShipperPhone,
                                    ROW_NUMBER() OVER(ORDER BY o.OrderID DESC) AS RowNumber
                            FROM    Orders as o
                                    LEFT JOIN Customers AS c ON o.CustomerID = c.CustomerID
                                    LEFT JOIN Employees AS e ON o.EmployeeID = e.EmployeeID
                                    LEFT JOIN Shippers AS s ON o.ShipperID = s.ShipperID
                            WHERE   (@status = 0 OR o.Status = @status)
                                AND (@searchValue = N'' OR c.CustomerName LIKE @searchValue OR s.ShipperName LIKE @searchValue)
                            ) AS t
                    WHERE (@pageSize = 0) OR (t.RowNumber BETWEEN(@page -1)*@pageSize + 1 AND @page*@pageSize)
                    ORDER BY t.RowNumber";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue,
                    status = status
                };

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters, null) ?? DBNull.Value);
                    }

                    using (var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(DataReaderToOrder(dbReader));
                        }
                    }
                }
                connection.Close();
            }

            return data;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<OrderDetail> ListDetails(int orderID)
        {
            List<OrderDetail> data = new List<OrderDetail>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT	od.*, p.ProductName, p.Unit, p.Photo		
                                    FROM	OrderDetails AS od
		                                    JOIN Products AS p ON od.ProductID = p.ProductID
                                    WHERE	od.OrderID = @orderID";
                var parameters = new
                {
                    orderID = orderID
                };
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;

                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        cmd.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(parameters, null) ?? DBNull.Value);
                    }

                    using (var dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(DataReaderToOrderDetail(dbReader));
                        }
                    }
                }

                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            int result = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"DECLARE @orderDetailID int;
                                    SELECT @orderDetailID = OrderDetailID FROM OrderDetails WHERE OrderID = @orderID AND ProductID = @productID;
                                    IF(@orderDetailID IS NULL)
                                        BEGIN
                                            INSERT INTO OrderDetails(OrderID, ProductID, Quantity, SalePrice)
                                            VALUES(@orderID, @productID, @quantity, @salePrice);
                                            SELECT SCOPE_IDENTITY();
                                        END
                                    ELSE
                                        BEGIN
                                            UPDATE OrderDetails SET Quantity = @quantity, SalePrice = @salePrice
                                            WHERE OrderDetailID = @orderDetailID;
                                            SELECT @orderDetailID;
                                        END";

                var parameters = new
                {
                    orderID = orderID,
                    productID = productID,
                    quantity = quantity,
                    salePrice = salePrice
                };

                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Update(Order data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Orders
                                    SET     CustomerID = @customerID,
                                            OrderTime = @orderTime,
                                            EmployeeID = @employeeID,
                                            AcceptTime = @acceptTime,
                                            ShipperID = @shipperID,
                                            ShippedTime = @shippedTime,
                                            FinishedTime = @finishedTime,
                                            Status = @status
                                    WHERE   OrderID = @orderID";

                var parameters = new
                {
                    orderID = data.OrderID,
                    customerID = data.CustomerID,
                    orderTime = data.OrderTime,
                    employeeID = data.EmployeeID,
                    acceptTime = data.AcceptTime ?? null,
                    shipperID = data.ShipperID ?? null,
                    shipperTime = data.ShippedTime,
                    finishedTime = data.FinishedTime,
                    status = data.Status
                };

                result = connection.Execute(sql, parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }
    }
}
