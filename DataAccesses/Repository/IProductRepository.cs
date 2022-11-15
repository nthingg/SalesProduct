using DataAccesses.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccesses.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void AddProduct(Product product);
        void RemoveProduct(int productId);
        void UpdateProduct(Product product);
        int GetProperNewProductID();
        Product GetProductById(int productId);
    }
}
