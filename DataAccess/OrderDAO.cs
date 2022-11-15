using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDAO : BaseDAL 
    {
        private static OrderDAO instance = null;
        private static readonly Object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new OrderDAO();
                }
                return instance;
            }
        }

        public IEnumerable<OrderObject> GetOrderList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderId, MemberId, OrderDate, RequiredDate, ShippedDate, Freight FROM [Order]";
            List<OrderObject> list = new List<OrderObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    OrderObject Order = new OrderObject
                    {
                        OrderID = dataReader.GetInt32(0),
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5)
                    };
                    list.Add(Order);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        public int GetSeed()
        {
            return GetSeed("Order");
        }
        public OrderObject GetOrderByID(int id)
        {
            OrderObject Order = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderID, MemberId, OrderDate, RequiredDate, ShippedDate, Freight"
                             + " FROM [Order] WHERE OrderID = @OrderID";
            try
            {
                var param = dataProvider.CreateParameter("@OrderID", 4, id, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    Order = new OrderObject
                    {
                        OrderID = id,
                        MemberID = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequiredDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                CloseConnection();
            }
            return Order;
        }
        public void AddOrder(OrderObject order)
        {
            try
            {
                string SQLUpdate = "INSERT [Order] VALUES (@MemberId, @OrderDate, @RequiredDate, @ShippedDate, @Freight)";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@MemberId", 4, order.MemberID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@OrderDate", 8, order.OrderDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@RequiredDate", 8, order.RequiredDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@ShippedDate", 8, order.ShippedDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@Freight", 16, order.Freight, DbType.Decimal));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void UpdateOrder(OrderObject order)
        {
            try
            {
                string SQLUpdate = "UPDATE [Order] SET MemberId = @MemberId, OrderDate = @OrderDate,"
                                 + " RequiredDate = @RequiredDate, ShippedDate = @ShippedDate, Freight = @Freight WHERE OrderId = @OrderID";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@MemberId", 4, order.MemberID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@OrderDate", 8, order.OrderDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@RequiredDate", 8, order.RequiredDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@ShippedDate", 8, order.ShippedDate, DbType.DateTime));
                parameters.Add(dataProvider.CreateParameter("@Freight", 16, order.Freight, DbType.Decimal));
                parameters.Add(dataProvider.CreateParameter("@OrderID",4, order.OrderID, DbType.Int32));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void RemoveOrder(int orderID)
        {
            try
            {
                string SQLUpdate = "DELETE [Order] WHERE OrderId = @OrderID";
                var parameter = dataProvider.CreateParameter("@OrderID", 4, orderID, DbType.Int32);
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
