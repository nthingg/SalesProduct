using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<OrderObject> GetAllOrders() => OrderDAO.Instance.GetOrderList();
        public void AddOrder(OrderObject Order) => OrderDAO.Instance.AddOrder(Order);
        public void RemoveOrder(int OrderId) => OrderDAO.Instance.RemoveOrder(OrderId);
        public void UpdateOrder(OrderObject Order) => OrderDAO.Instance.UpdateOrder(Order);
        public int GetProperNewOrderID() => OrderDAO.Instance.GetSeed();
    }
}
