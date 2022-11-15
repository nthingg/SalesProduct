using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetailObject orderDetail);
        void RemoveOrderDetail(OrderDetailObject orderDetail);
        IEnumerable<OrderDetailObject> GetOrderDetailByID(int id);
        void RemoveAllDetail(int orderID);
        OrderDetailObject GetDuplicateOrderDetail(int orderID, int productID);
    }
}
