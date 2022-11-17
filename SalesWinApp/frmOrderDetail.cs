
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
    public partial class frmOrderDetail : Form
    {
        public frmOrderDetail()
        {
            InitializeComponent();
        }
        private List<OrderDetail> orderDetails;
        private IProductRepository productRepository = new ProductRepository();
        private IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        private BindingSource source;
        private Order order;
        private bool isAdmin;
        private int orderId;
        public int OrderId { get => orderId; set => orderId = value; }
        public Order Order { set => order = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }
        public List<OrderDetail> OrderDetails { get => orderDetails; set => orderDetails = value; }

        private void frmOrderDetail_Load(object sender, EventArgs e)
        {
            lbOrderID.Text = "Order #" + OrderId;
            LoadOrderDetailList();
            if (!isAdmin)
            {
                btnAdd.Visible = false;
                btnUpdate.Visible= false;
                btnRemove.Visible = false;
                btnSave.Visible = false;
                btnCancel.Text = "OK";
            }
           
        }
        private void LoadOrderDetailList()
        {
            var list = orderDetails;
            try
            {
                source = new BindingSource();
                source.DataSource = list;
                dvgData.DataSource = null;
                dvgData.DataSource = source;
                this.dvgData.Columns["Order"].Visible = false;
                this.dvgData.Columns["Product"].Visible = false;
                if (list.Count() == 0)
                {
                    btnRemove.Enabled = false;
                    btnUpdate.Enabled = false;
                }
                else
                {
                    dvgData.Columns[0].Visible = false;
                    btnRemove.Enabled = true;
                    btnUpdate.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load product list");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {         
            frmAddOrderDetail addDetailForm = new frmAddOrderDetail
            {
                InsertOrUpdate = true,
                Order = order,
                OrderDetails = orderDetails
            };
            if (addDetailForm.ShowDialog() == DialogResult.OK)
            {
                OrderDetail orderDetail = addDetailForm.OrderDetail;
                orderDetails.Add(orderDetail);
                LoadOrderDetailList();               
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OrderDetail orderDetail = source.Current as OrderDetail;
            frmAddOrderDetail updateDetailForm = new frmAddOrderDetail
            {
                InsertOrUpdate = false,
                Order = order,
                OrderDetails = orderDetails,
                OrderDetail = orderDetail
            };
            if (updateDetailForm.ShowDialog() == DialogResult.OK)
            {
                int pos = orderDetails.IndexOf(orderDetail);
                orderDetails.Remove(orderDetail);
                orderDetails.Insert(pos, updateDetailForm.OrderDetail);
                LoadOrderDetailList();
                source.Position = pos;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            OrderDetail orderDetail = dvgData.CurrentRow.DataBoundItem as OrderDetail;
            DialogResult d = MessageBox.Show("Do you really want to remove this order detail ?", "Order Detail Management - Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (d == DialogResult.Yes)
            {
                orderDetails.Remove(orderDetail);
                LoadOrderDetailList();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Are you sure to save all changes ?", "Order Detail Management - Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (d == DialogResult.No) DialogResult = DialogResult.Cancel;
            if (d == DialogResult.Cancel) DialogResult = DialogResult.None;
            else
            {
                foreach (var detail in orderDetails)
                {   
                    Product product = productRepository.GetProductById(detail.ProductId);
                    OrderDetail orderDetail = orderDetailRepository.GetDuplicateOrderDetail(detail.OrderId, detail.ProductId);

                    if (orderDetail != null)
                    {
                        int quantity = 0;
                        if (detail.Quantity < orderDetail.Quantity)
                        {
                            quantity = orderDetail.Quantity - detail.Quantity;
                            product.UnitInStock += quantity;
                        }
                        else
                        {
                            quantity = detail.Quantity - orderDetail.Quantity;
                            product.UnitInStock -= quantity;
                        }

                        productRepository.UpdateProduct(product);
                    }                    
                }
            } 
        }
    }
}
