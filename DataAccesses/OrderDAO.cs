using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses
{
    public class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly Object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new OrderDAO();
                }
                return instance;
            }
        }

        public IEnumerable<Order> GetOrderList()
        {
            var orders = new List<Order>();
            try
            {
                using var context = new SalesProductContext();
                orders = context.Orders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public int GetSeed()
        {
            var orders = new List<Order>();
            try
            {
                using var context = new SalesProductContext();
                orders = context.Orders.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            int id = 0;
            if (orders.Count == 0)
            {
                id = 1;
            }
            else
            {
                var order = orders.Last();
                id = order.OrderId + 1;
            }
            
            return id;
        }

        public Order GetOrderByID(int id)
        {
            Order order = null;
            try
            {
                using var context = new SalesProductContext();
                order = context.Orders.SingleOrDefault(c => c.OrderId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public void AddOrder(Order order)
        {
            try
            {
                using var context = new SalesProductContext();
                context.Orders.Add(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateOrder(Order order)
        {
            try
            {
                using var context = new SalesProductContext();
                context.Orders.Update(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveOrder(int orderID)
        {
            try
            {
                Order order = GetOrderByID(orderID);
                using var context = new SalesProductContext();
                context.Orders.Remove(order);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
