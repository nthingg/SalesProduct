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

namespace SalesWinApp
{
    public partial class frmAddOrderDetail : Form
    {
        public frmAddOrderDetail()
        {
            InitializeComponent();
        }
        private IProductRepository productRepository = new ProductRepository();
        private List<OrderDetail> orderDetails;
        private OrderDetail orderDetail;
        private Order order;
        private bool insertOrUpdate;
        public OrderDetail OrderDetail { get => orderDetail; set => orderDetail = value; }
        public bool InsertOrUpdate { set => insertOrUpdate = value; }

        public Order Order { set => order = value; }
        public List<OrderDetail> OrderDetails { set => orderDetails = value; }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "Update")
            {
                if (txtQuantity.Text != "" && cboProduct.SelectedItem.ToString().Split('.')[0] != "-- Select --")
                {
                    int quantity = int.Parse(txtQuantity.Text);
                    if (!validateQuantityUpdate(quantity))
                    {
                        MessageBox.Show("Unit in stock is less than required update", "Update Detail - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtQuantity.Clear();
                    }
                    else
                    {
                        int productID = int.Parse(cboProduct.SelectedItem.ToString().Split('.')[0]);
                        orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = productID,
                            ProductName = productRepository.GetProductById(productID).ProductName,
                            UnitPrice = Decimal.Parse(txtUnitPrice.Text),
                            Quantity = int.Parse(txtQuantity.Text),
                            Discount = Double.Parse(txtDiscount.Text)

                        };
                        DialogResult = DialogResult.OK;
                    }
                }
            }
            else
            {
                if (txtQuantity.Text != "" && cboProduct.SelectedItem.ToString().Split('.')[0] != "-- Select --")
                {
                    int quantity = int.Parse(txtQuantity.Text);
                    if (!validateQuantity(quantity))
                    {
                        MessageBox.Show("Unit in stock is less than required add", "Update Detail - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtQuantity.Clear();
                    } 
                    else
                    {
                        int productID = int.Parse(cboProduct.SelectedItem.ToString().Split('.')[0]);
                        orderDetail = new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = productID,
                            ProductName = productRepository.GetProductById(productID).ProductName,
                            UnitPrice = Decimal.Parse(txtUnitPrice.Text),
                            Quantity = int.Parse(txtQuantity.Text),
                            Discount = Double.Parse(txtDiscount.Text)
                        };
                        DialogResult = DialogResult.OK;
                    }
                }
            }

            
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            cboProduct.SelectedIndex = 0;
            txtQuantity.Clear();
            txtDiscount.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private void frmAddOrderDetail_Load(object sender, EventArgs e)
        {
            var productList = productRepository.GetAllProducts();
            if (orderDetails.Count > 0)
            {
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    productList = productList.Where(product => product.ProductId != orderDetail.ProductId);
                }

            }
            LoadProductComboBox(productList);
            if (!insertOrUpdate)
            {
                String item = orderDetail.ProductId.ToString() + ". " + orderDetail.ProductName;
                cboProduct.Items.Insert(1, item);
                cboProduct.SelectedIndex = 1;
                txtQuantity.Text = orderDetail.Quantity.ToString();
                txtDiscount.Text = orderDetail.Discount.ToString();    
                cboProduct.Enabled = false;
                btnAdd.Text = "Update";
            } else
            {

            }
        }
        private void LoadProductComboBox(IEnumerable<Product> list)
        {
            cboProduct.Items.Insert(0, "-- Select --");
            foreach (Product product in list)
            {
                String item = product.ProductId.ToString() + ". " + product.ProductName;
                cboProduct.Items.Add(item);
            }
            cboProduct.SelectedIndex = 0;
        }

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProduct.SelectedIndex != 0)
            {
                int productId = int.Parse(cboProduct.SelectedItem.ToString().Split('.')[0]);
                Product product = productRepository.GetProductById(productId);
                txtUnitPrice.Text = product.UnitPrice.ToString();
            }
            validateForm();
        }
        private void validateForm()
        {
            bool enabled = true;
            if (cboProduct.SelectedIndex == 0) enabled = false;
            if (!validateInteger(txtQuantity.Text)) enabled = false;
            if (!validateFloat(txtDiscount.Text)) enabled = false;
            btnAdd.Enabled = enabled;
        }
        private bool validateInteger(String Integer)
        {
            String regex = "^[0-9]+$";
            return Regex.IsMatch(Integer, regex);
        }
        private bool validateFloat(String Integer)
        {
            String regex = "^\\d+(\\.)?\\d*$";
            return Regex.IsMatch(Integer, regex);
        }
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private bool validateQuantityUpdate(int quantity)
        {
            if (cboProduct.SelectedItem.ToString().Split('.')[0] != "-- Select --")
            {
                int productID = int.Parse(cboProduct.SelectedItem.ToString().Split('.')[0]);
                Product product = productRepository.GetProductById(productID);
                if (quantity <= product.UnitInStock + orderDetail.Quantity)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool validateQuantity(int quantity)
        {   
            if (cboProduct.SelectedItem.ToString().Split('.')[0] != "-- Select --")
            {
                int productID = int.Parse(cboProduct.SelectedItem.ToString().Split('.')[0]);
                Product product = productRepository.GetProductById(productID);
                if (quantity <= product.UnitInStock)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void txtQuantity_Validated(object sender, EventArgs e)
        {
        }

        private void txtQuantity_Validating(object sender, CancelEventArgs e)
        {   
        }
    }
}
