using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<ProductObject> GetAllProducts() => ProductDAO.Instance.GetProductList();
        public void AddProduct(ProductObject product) => ProductDAO.Instance.AddProduct(product);
        public void RemoveProduct(int productId) => ProductDAO.Instance.RemoveProduct(productId);
        public void UpdateProduct(ProductObject product) => ProductDAO.Instance.UpdateProduct(product);
        public int GetProperNewProductID() => ProductDAO.Instance.GetSeed();
        public ProductObject GetProductById(int productId) => ProductDAO.Instance.GetProductByID(productId);
    }
}
