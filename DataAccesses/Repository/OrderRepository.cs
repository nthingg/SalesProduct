using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetAllOrders() => OrderDAO.Instance.GetOrderList();
        public void AddOrder(Order Order) => OrderDAO.Instance.AddOrder(Order);
        public void RemoveOrder(int OrderId) => OrderDAO.Instance.RemoveOrder(OrderId);
        public void UpdateOrder(Order Order) => OrderDAO.Instance.UpdateOrder(Order);
        public int GetProperNewOrderID() => OrderDAO.Instance.GetSeed();
    }
}
