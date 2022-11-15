using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail orderDetail);
        void RemoveOrderDetail(OrderDetail orderDetail);
        IEnumerable<OrderDetail> GetOrderDetailByID(int id);
        void RemoveAllDetail(int orderID);
        OrderDetail GetDuplicateOrderDetail(int orderID, int productID);
    }
}
