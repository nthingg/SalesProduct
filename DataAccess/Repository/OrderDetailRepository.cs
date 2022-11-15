using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {

        public void AddOrderDetail(OrderDetailObject orderDetail) => OrderDetailDAO.Instance.AddOrderDetail(orderDetail);
        public void RemoveOrderDetail(OrderDetailObject orderDetail) => OrderDetailDAO.Instance.RemoveOrderDetail(orderDetail);
        public IEnumerable<OrderDetailObject> GetOrderDetailByID(int id) => OrderDetailDAO.Instance.GetOrderDetailByID(id);
        public void RemoveAllDetail(int orderID) => OrderDetailDAO.Instance.RemoveAllDetail(orderID);
        public OrderDetailObject GetDuplicateOrderDetail(int orderID, int productID) => OrderDetailDAO.Instance.GetDuplicateOrderDetail(orderID, productID);
    }
}
