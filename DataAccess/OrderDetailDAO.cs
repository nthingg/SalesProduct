using BusinessObject;
using DataAccess.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class OrderDetailDAO : BaseDAL
    {
        private static OrderDetailDAO instance = null;
        private static readonly Object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new OrderDetailDAO();
                }
                return instance;
            }
        }       
        public IEnumerable<OrderDetailObject> GetOrderDetailByID(int id)
        {
            List<OrderDetailObject> list = new List<OrderDetailObject>();
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderID, ProductId, UnitPrice, Quantity, Discount"
                             + " FROM OrderDetail WHERE OrderID = @OrderID";
            try
            {
                var param = dataProvider.CreateParameter("@OrderID", 4, id, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                while (dataReader.Read())
                {
                    int productID = dataReader.GetInt32(1);
                    IProductRepository repo = new ProductRepository();
                    ProductObject product = repo.GetProductById(productID);
                    OrderDetailObject OrderDetail = new OrderDetailObject
                    {
                        OrderID = id,
                        ProductID = productID,
                        ProductName = product.ProductName,
                        UnitPrice= dataReader.GetDecimal(2),
                        Discount = dataReader.GetDouble(4),
                        Quantity = dataReader.GetInt32(3)
                    };
                    list.Add(OrderDetail);
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
            return list;
        }
        public OrderDetailObject GetDuplicateOrderDetail(int orderID, int productID)
        {
            OrderDetailObject orderDetail = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderId, ProductId, UnitPrice, Quantity, Discount"
                             + " FROM OrderDetail WHERE OrderId = @OrderID AND ProductId = @ProductID";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@OrderID", 4, orderID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductID", 4, productID, DbType.Int32));
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, parameters.ToArray());
                if (dataReader.Read())
                {
                    IProductRepository repo = new ProductRepository();
                    ProductObject product = repo.GetProductById(productID);
                    orderDetail = new OrderDetailObject
                    {
                        OrderID = orderID,
                        ProductID = productID,
                        ProductName = product.ProductName,
                        UnitPrice = dataReader.GetDecimal(2),
                        Discount = dataReader.GetDouble(4),
                        Quantity = dataReader.GetInt32(3)
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
            return orderDetail;
        }
        public void AddOrderDetail(OrderDetailObject OrderDetail)
        {
            try
            {
                string SQLUpdate = "INSERT OrderDetail VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@OrderId", 4, OrderDetail.OrderID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductId", 4, OrderDetail.ProductID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@UnitPrice", 16, OrderDetail.UnitPrice, DbType.Decimal)); ;
                parameters.Add(dataProvider.CreateParameter("@Quantity", 4, OrderDetail.Quantity, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@Discount", 8, OrderDetail.Discount, DbType.Double));
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
        public void RemoveOrderDetail(OrderDetailObject orderDetail)
        {
            try
            {
                string SQLUpdate = "DELETE OrderDetail WHERE OrderId = @OrderID AND ProductId = @ProductID";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@OrderID", 4, orderDetail.OrderID, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductID", 4, orderDetail.ProductID, DbType.Int32));
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
        public void RemoveAllDetail(int orderID)
        {
            try
            {
                string SQLUpdate = "DELETE OrderDetail WHERE OrderId = @OrderID";
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
