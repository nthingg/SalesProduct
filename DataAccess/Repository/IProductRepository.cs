using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<ProductObject> GetAllProducts();
        void AddProduct(ProductObject product);
        void RemoveProduct(int productId);
        void UpdateProduct(ProductObject product);
        int GetProperNewProductID();
        ProductObject GetProductById(int productId);
    }
}
