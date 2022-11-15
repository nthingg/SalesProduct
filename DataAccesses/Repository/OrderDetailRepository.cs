using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void AddOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.Instance.AddOrderDetail(orderDetail);
        public void RemoveOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.Instance.RemoveOrderDetail(orderDetail);
        public IEnumerable<OrderDetail> GetOrderDetailByID(int id) => OrderDetailDAO.Instance.GetOrderDetailByID(id);
        public void RemoveAllDetail(int orderID) => OrderDetailDAO.Instance.RemoveAllDetail(orderID);
        public OrderDetail GetDuplicateOrderDetail(int orderID, int productID) => OrderDetailDAO.Instance.GetDuplicateOrderDetail(orderID, productID);
    }
}
