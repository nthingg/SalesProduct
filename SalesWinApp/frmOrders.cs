using DataAccesses.DataAccess;
using DataAccesses.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWinApp
{
    public partial class frmOrders : Form
    {
        public frmOrders()
        {
            InitializeComponent();
        }
        private Member member;
        private IOrderRepository orderRepository = new OrderRepository();
        private IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        private BindingSource source;

        public IOrderRepository OrderRepository { set => orderRepository = value; }
        public Member Member { set => member = value; }

        private void frmOrders_Load(object sender, EventArgs e)
        {
            orderDetailRepository.GetOrderDetailByID(0);
            var orders = orderRepository.GetAllOrders();
            if (!member.IsAdmin) orders = orders.Where(order => order.MemberId == member.MemberId);
            LoadOrderList(orders);
            btnDetail.Text = "Update detail";
            if (!member.IsAdmin)
            {
                btnDetail.Text = "View Detail";
                btnAdd.Visible= false;
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
            }
        }
        public void LoadOrderList(IEnumerable<Order> orderList)
        {
            var orders = orderList;            
            try
            {
                source = new BindingSource();
                source.DataSource = orders;
                dvgData.DataSource = null;
                dvgData.DataSource = source;
                this.dvgData.Columns["Member"].Visible = false;
                this.dvgData.Columns["OrderDetails"].Visible = false;
                if (orders.Count() > 0)
                {
                    for (int i = 2; i < 5; i++)
                    {
                        dvgData.Columns[i].DefaultCellStyle.Format = " HH:mm:ss dd/MM/yyyy";
                    }
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnDetail.Visible = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                    btnUpdate.Enabled = false;
                    btnDetail.Visible = false;
                    dvgData.Visible = false;
                    MessageBox.Show("No order history.", "Order Management",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Order list");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddOrder addOrderForm = new frmAddOrder
            {
                OrderRepository = orderRepository,
                InsertOrUpdate = true
            };
            if (addOrderForm.ShowDialog() == DialogResult.OK)
            {
                LoadOrderList(orderRepository.GetAllOrders());
                source.Position = source.Count - 1;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Order order = dvgData.CurrentRow.DataBoundItem as Order;
            frmAddOrder updateOrderForm = new frmAddOrder
            {              
                OrderRepository = orderRepository,
                InsertOrUpdate = false,
                OrderInfo = order 
            };
            if (updateOrderForm.ShowDialog() == DialogResult.OK)
            {
                int index = dvgData.CurrentRow.Index;
                LoadOrderList(orderRepository.GetAllOrders());
                source.Position = index;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Order order = dvgData.CurrentRow.DataBoundItem as Order;
            DialogResult d = MessageBox.Show("Do you really want to remove this order ?", "Order Management - Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (d == DialogResult.Yes)
            {
                orderDetailRepository.RemoveAllDetail(order.OrderId);
                orderRepository.RemoveOrder(order.OrderId);
                LoadOrderList(orderRepository.GetAllOrders());
            }
        }
        private void btnDetail_Click(object sender, EventArgs e)
        {
            Order order = dvgData.CurrentRow.DataBoundItem as Order;
            frmOrderDetail orderDetailForm = new frmOrderDetail
            {   
                OrderId = order.OrderId,
                Order = order,
                IsAdmin = member.IsAdmin,
                OrderDetails = orderDetailRepository.GetOrderDetailByID(order.OrderId).ToList()
            };
            if (orderDetailForm.ShowDialog() == DialogResult.OK)
            {
                int index = dvgData.CurrentRow.Index;
                List<OrderDetail> orderDetails = orderDetailForm.OrderDetails;
                orderDetailRepository.RemoveAllDetail(order.OrderId);
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    orderDetailRepository.AddOrderDetail(orderDetail);
                }
                LoadOrderList(orderRepository.GetAllOrders());
                source.Position = index;
            }
        }

    }
}
