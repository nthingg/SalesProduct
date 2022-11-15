using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();
        void AddOrder(Order Order);
        void RemoveOrder(int OrderId);
        void UpdateOrder(Order Order);
        int GetProperNewOrderID();
    }
}
