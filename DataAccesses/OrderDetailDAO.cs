using DataAccesses.DataAccess;
using System.Diagnostics.Metrics;

namespace DataAccesses
{
    public class OrderDetailDAO
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

        public IEnumerable<OrderDetail> GetOrderDetailByID(int id)
        {
            var details = new List<OrderDetail>();
            try
            {
                using var context = new SalesProductContext();
                details = context.OrderDetails.Where(c => c.OrderId == id).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return details;
        }

        public OrderDetail GetDuplicateOrderDetail(int orderID, int productID)
        {
            OrderDetail detail = null;
            try
            {
                using var context = new SalesProductContext();
                detail = context.OrderDetails.SingleOrDefault(c => c.OrderId == orderID && c.ProductId == productID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return detail;
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using var context = new SalesProductContext();
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
            try
            {
                using var context = new SalesProductContext();
                context.OrderDetails.Remove(orderDetail);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveAllDetail(int orderID)
        {
            try
            {
                using var context = new SalesProductContext();

                foreach (var entity in context.OrderDetails)
                {
                    context.OrderDetails.Remove(entity);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
