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
    public partial class frmAddProduct : Form
    {
        public frmAddProduct()
        {
            InitializeComponent();
        }
        private IProductRepository productRepository;
        private Product productInfo;
        public bool InsertOrUpdate { get; set; }
        public IProductRepository ProductRepository {set => productRepository = value; }
        public Product ProductInfo {set => productInfo = value; }

        private void frmAddProduct_Load(object sender, EventArgs e)
        {
            txtProductId.Text = (productRepository.GetProperNewProductID()).ToString();
            if (!InsertOrUpdate)
            {
                this.Text = "Update Product";
                txtProductId.Text = productInfo.ProductId.ToString();
                txtProductName.Text = productInfo.ProductName;
                txtCategoryId.Text = productInfo.CategoryId.ToString();
                txtWeight.Text = productInfo.Weight;
                txtUnitPrice.Text = productInfo.UnitPrice.ToString();
                txtStock.Text = productInfo.UnitInStock.ToString();
                btnAdd.Text = "Update";
                btnAdd.Enabled = true;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtProductName.Clear();
            txtCategoryId.Clear();
            txtWeight.Clear();
            txtUnitPrice.Clear();
            txtStock.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
        private void validateForm()
        {
            String ProductName = txtProductName.Text;
            String CategoryId = txtCategoryId.Text;
            String Weight = txtWeight.Text;
            String UnitPrice = txtUnitPrice.Text;
            String InStock = txtStock.Text;
            bool enabled = true;
            if (!validateInteger(CategoryId)) enabled = false;
            if (!validateFloat(UnitPrice)) enabled = false;
            if (!validateInteger(InStock)) enabled = false;
            btnAdd.Enabled = enabled;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Product product = new Product
            {
                ProductName = txtProductName.Text,
                CategoryId = int.Parse(txtCategoryId.Text),
                UnitPrice = Decimal.Parse(txtUnitPrice.Text),
                Weight = txtWeight.Text,
                UnitInStock = int.Parse(txtStock.Text)
            };

            if (btnAdd.Text == "Add")
            {
                productRepository.AddProduct(product);
            } else
            {
                product.ProductId = int.Parse(txtProductId.Text);
                productRepository.UpdateProduct(product);
            }
            
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

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }
        private void txtCategoryId_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtCategoryId_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtCategoryId, "");
        }

        private void txtCategoryId_Validating(object sender, CancelEventArgs e)
        {
            String num = txtCategoryId.Text;
            if (!validateInteger(num))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCategoryId, "Only allow digits");
            }
        }

        private void txtUnitPrice_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtUnitPrice, "");
        }

        private void txtUnitPrice_Validating(object sender, CancelEventArgs e)
        {
            String num = txtUnitPrice.Text;
            if (!validateFloat(num))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUnitPrice, "Only allow digits");
            }
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {
            validateForm();
        }

        private void txtStock_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtStock, "");
        }

        private void txtStock_Validating(object sender, CancelEventArgs e)
        {
            String num = txtStock.Text;
            if (!validateInteger(num))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtStock, "Only allow digits");
            }
        }
    }
}
