using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderObject> GetAllOrders();
        void AddOrder(OrderObject Order);
        void RemoveOrder(int OrderId);
        void UpdateOrder(OrderObject Order);
        int GetProperNewOrderID();
    }
}
