using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAllProducts() => ProductDAO.Instance.GetProductList();
        public void AddProduct(Product product) => ProductDAO.Instance.AddProduct(product);
        public void RemoveProduct(int productId) => ProductDAO.Instance.RemoveProduct(productId);
        public void UpdateProduct(Product product) => ProductDAO.Instance.UpdateProduct(product);
        public int GetProperNewProductID() => ProductDAO.Instance.GetSeed();
        public Product GetProductById(int productId) => ProductDAO.Instance.GetProductByID(productId);
    }
}
