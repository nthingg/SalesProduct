using DataAccesses.DataAccess;
using DataAccesses.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SalesWinApp
{
    public partial class frmAddOrder : Form
    {
        public frmAddOrder()
        {
            InitializeComponent();
        }
        private IMemberRepository memberRepository = new MemberRepository();
        private IOrderRepository orderRepository;
        private IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        private IProductRepository productRepository = new ProductRepository();
        private bool insertOrUpdate;
        private Order orderInfo;
        public IOrderRepository OrderRepository { set => orderRepository = value; }
        public bool InsertOrUpdate { set => insertOrUpdate = value; }
        public Order OrderInfo { set => orderInfo = value; }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Order order = new Order
            {
                MemberId = int.Parse(txtMemberID.Text),
                OrderDate = orderDatePicker.Value,
                RequiredDate = requiredDatePicker.Value,
                ShippedDate = shippedDatePicker.Value,
                Freight = Decimal.Parse(txtFreight.Text)
            };
            if (validateID())
            {
                if (insertOrUpdate)
                {
                    frmOrderDetail frmOrderDetail = new frmOrderDetail
                    {
                        OrderId = int.Parse(txtOrderID.Text),
                        Order = order,
                        OrderDetails = new List<OrderDetail>(),
                        IsAdmin = true,
                    };
                    if (frmOrderDetail.ShowDialog() == DialogResult.OK)
                    {
                        orderRepository.AddOrder(order);
                        IEnumerable<OrderDetail> orderDetails = frmOrderDetail.OrderDetails;
                        foreach (OrderDetail detail in orderDetails)
                        {
                            detail.OrderId = int.Parse(txtOrderID.Text);
                            orderDetailRepository.AddOrderDetail(detail);

                            Product product = productRepository.GetProductById(detail.ProductId);
                            int quantity = 0;
                            quantity = product.UnitInStock - detail.Quantity;
                            product.UnitInStock = quantity;
                            productRepository.UpdateProduct(product);

                        }
                        DialogResult = DialogResult.OK;
                    }
                    else {
                        DialogResult = DialogResult.Cancel;
                    }
                } else
                {
                    order.OrderId = int.Parse(txtOrderID.Text);
                    orderRepository.UpdateOrder(order);
                    DialogResult = DialogResult.OK;
                } 
            } else
            {
                MessageBox.Show("MemberID not existed", "Add New Order - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMemberID.Clear();
            }
        }

        private void frmAddOrder_Load(object sender, EventArgs e)
        {
            foreach(DateTimePicker timePicker in this.Controls.OfType<DateTimePicker>())
            {
                timePicker.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
                timePicker.Format = DateTimePickerFormat.Custom;
            }
            if (insertOrUpdate)
            {
                txtOrderID.Text = (orderRepository.GetProperNewOrderID()).ToString();
                orderDatePicker.Value = DateTime.Now;
                requiredDatePicker.Value = DateTime.Now;
                shippedDatePicker.Value = DateTime.Now;
            } else
            {
                this.Text = "Update Order";
                txtOrderID.Text = orderInfo.OrderId.ToString();
                txtMemberID.Text = orderInfo.MemberId.ToString();
                orderDatePicker.Value = orderInfo.OrderDate;
                requiredDatePicker.Value = orderInfo.RequiredDate ?? DateTime.Now;
                shippedDatePicker.Value = orderInfo.ShippedDate ?? DateTime.Now;
                txtFreight.Text = orderInfo.Freight.ToString();
                btnAdd.Text = "Update";
            }
            
            
        }
        private bool validateInteger(String Integer)
        {
            String regex = "^[0-9]+$";
            return Regex.IsMatch(Integer, regex);
        }
        private bool validateID()
        {
            if (memberRepository.GetMember(int.Parse(txtMemberID.Text)) != null) return true;
            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private bool validateDecimal(String Integer)
        {
            String regex = "^\\d+(\\.)?\\d*$";
            return Regex.IsMatch(Integer, regex);
        }
        private void validateForm()
        {
            bool enabled = true;
            if (!validateInteger(txtMemberID.Text)) enabled = false;
            if (!validateDecimal(txtFreight.Text)) enabled = false;
            btnAdd.Enabled = enabled;
        }

        private void GetMemberName(int id)
        {
            Member member = memberRepository.GetMember(id);
            if (member != null)
            {
                txtMemberName.Text = member.MemberName;
            } else
            {
                txtMemberName.Text = "";
            }
        }

        private void txtMemberID_TextChanged(object sender, EventArgs e)
        {
            validateForm();
            if (txtMemberID.Text == "")
            {
                txtMemberName.Text = "";
            } 
            else
            {   
                if (validateInteger(txtMemberID.Text))
                {
                    GetMemberName(int.Parse(txtMemberID.Text));
                }
                
            }
            
        }

        private void txtMemberID_Validated(object sender, EventArgs e)
        {

        }

        private void txtMemberID_Validating(object sender, CancelEventArgs e)
        {

        }

        private void txtFreight_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtFreight_Validated(object sender, EventArgs e)
        {

        }

        private void txtFreight_Validating(object sender, CancelEventArgs e)
        {

        }
    }
}
