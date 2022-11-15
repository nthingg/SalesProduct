
using DataAccesses.DataAccess;
using DataAccesses.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SalesWinApp
{
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }
        private IProductRepository productRepository = new ProductRepository();
        private BindingSource source;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddProduct addProductForm = new frmAddProduct
            {
                ProductRepository = productRepository,
                InsertOrUpdate = true
            };
            if (addProductForm.ShowDialog() == DialogResult.OK)
            {
                LoadProductList(productRepository.GetAllProducts());
                loadProductFilter();
                source.Position = source.Count - 1;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Product product = dvgData.CurrentRow.DataBoundItem as Product;
            frmAddProduct updateProductForm = new frmAddProduct
            {
                ProductRepository = productRepository,
                InsertOrUpdate = false,
                ProductInfo = product
                
            };
            if (updateProductForm.ShowDialog() == DialogResult.OK)
            {
                int index = dvgData.CurrentRow.Index;
                LoadProductList(productRepository.GetAllProducts());
                loadProductFilter();
                source.Position = index;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Product product = dvgData.CurrentRow.DataBoundItem as Product;
            productRepository.RemoveProduct(product.ProductId);
            LoadProductList(productRepository.GetAllProducts());
            loadProductFilter();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProductID.Clear();
            txtProductName.Clear();
            txtMinPrice.Value = txtMinPrice.Minimum;
            txtMaxPrice.Value = txtMaxPrice.Maximum;
            txtStock.Value = txtStock.Minimum;
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            LoadProductList(productRepository.GetAllProducts());
            loadProductFilter();                     
        }
        private void loadProductFilter()
        {
            txtMinPrice.Minimum = productRepository.GetAllProducts().Select(product => product.UnitPrice).Min();
            txtMaxPrice.Maximum = productRepository.GetAllProducts().Select(product => product.UnitPrice).Max();
            txtStock.Minimum = productRepository.GetAllProducts().Select(product => product.UnitInStock).Min();
            txtMinPrice.Maximum = txtMaxPrice.Maximum;
            txtMaxPrice.Value = txtMaxPrice.Maximum;
            txtMaxPrice.Minimum = txtMinPrice.Minimum;
            txtStock.Maximum = productRepository.GetAllProducts().Select(product => product.UnitInStock).Max();
        }
        public void LoadProductList(IEnumerable<Product> productList)
        {
            var products = productList;           
            try
            {
                source = new BindingSource();
                source.DataSource = products;
                dvgData.DataSource = null;
                dvgData.DataSource = source;
                this.dvgData.Columns["OrderDetails"].Visible = false;
                if (products.Count() == 0)
                {
                    btnDelete.Enabled = false;
                    btnUpdate.Enabled = false;
                }
                else
                {
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load product list");
            }
        }      
        private List<Product> filterProduct()
        {
            var result = productRepository.GetAllProducts();
            if (txtProductID.Text.Length > 0) result = result.Where(product => product.ProductId.ToString().Contains(txtProductID.Text));
            if (txtProductName.Text.Length > 0) result = result.Where(product => product.ProductName.ToUpper().Contains(txtProductName.Text.ToUpper()));           
            if (txtMinPrice.Value > txtMinPrice.Minimum) result = result.Where(product => product.UnitPrice >= txtMinPrice.Value);
            if (txtMaxPrice.Value < txtMaxPrice.Maximum) result = result.Where(product => product.UnitPrice <= txtMaxPrice.Value);
            if (txtStock.Value > txtStock.Minimum) result = result.Where((product) => product.UnitInStock >= txtStock.Value);
            return result.ToList();
        }
        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            LoadProductList(filterProduct());
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            LoadProductList(filterProduct());
        }   
        private void txtMinPrice_ValueChanged(object sender, EventArgs e)
        {
            LoadProductList(filterProduct());
        }

        private void txtMaxPrice_ValueChanged(object sender, EventArgs e)
        {
            LoadProductList(filterProduct());
        }

        private void txtStock_ValueChanged(object sender, EventArgs e)
        {
            LoadProductList(filterProduct());
        }
    }
}
