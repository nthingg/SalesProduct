using BusinessObject;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class ProductDAO : BaseDAL
    {
        private static ProductDAO instance = null;
        private static readonly Object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null) instance = new ProductDAO();
                }
                return instance;
            }
        }

        public IEnumerable<ProductObject> GetProductList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT ProductID, CategoryId, ProductName, Weight, UnitPrice, UnitInStock FROM Product";
            List<ProductObject> list = new List<ProductObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    ProductObject Product = new ProductObject
                    {
                        ProductId = dataReader.GetInt32(0),
                        ProductCategoryId = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitInStock = dataReader.GetInt32(5)
                    };
                    list.Add(Product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null) dataReader.Close();
                CloseConnection();
            }
            return list;
        }
        public int GetSeed()
        {          
            return GetSeed("Product");
        }
        public ProductObject GetProductByID(int id)
        {
            ProductObject Product = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT ProductID, CategoryId, ProductName, Weight, UnitPrice, UnitInStock"
                             + " FROM Product WHERE ProductID = @ProductID";
            try
            {
                var param = dataProvider.CreateParameter("@ProductID", 4, id, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    Product = new ProductObject
                    {
                        ProductId = id,
                        ProductCategoryId = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitInStock = dataReader.GetInt32(5)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                CloseConnection();
            }
            return Product;
        }
        public void AddProduct(ProductObject Product)
        {
            try
            {
                string SQLUpdate = "INSERT Product VALUES (@CategoryId, @ProductName, @Weight, @UnitPrice, @UnitInStock)";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@CategoryId", 4, Product.ProductCategoryId, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductName", 50, Product.ProductName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Weight", 50, Product.Weight, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@UnitPrice", 16, Product.UnitPrice, DbType.Decimal));
                parameters.Add(dataProvider.CreateParameter("@UnitInStock", 16, Product.UnitInStock, DbType.Int32));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void UpdateProduct(ProductObject Product)
        {
            try
            {
                string SQLUpdate = "UPDATE Product SET CategoryId = @CategoryId, ProductName = @ProductName,"
                                 + " Weight = @Weight, UnitPrice = @UnitPrice, UnitInStock = @UnitInStock WHERE ProductId = @ProductID";
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@CategoryId", 4, Product.ProductCategoryId, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductName", 50, Product.ProductName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Weight", 50, Product.Weight, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@UnitPrice", 16, Product.UnitPrice, DbType.Decimal));
                parameters.Add(dataProvider.CreateParameter("@UnitInStock", 4, Product.UnitInStock, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductID", 4, Product.ProductId, DbType.Int32));
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
        public void RemoveProduct(int productId)
        {
            try
            {
                string SQLUpdate = "DELETE Product WHERE ProductId = @ProductID";
                var parameter = dataProvider.CreateParameter("@ProductID", 4, productId, DbType.Int32);
                dataProvider.UpdateDB(SQLUpdate, CommandType.Text, parameter);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }         
    }
}