using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BaseDAL
    {
        public StockDataProvider dataProvider { get; set; } = null;
        public SqlConnection connection = null;

        public BaseDAL()
        {
            var connectionString = GetConnectionString();          
            dataProvider = new StockDataProvider(connectionString);
        }
        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();          
            string connectionString = config["ConnectionString:Fstore"];           
            return connectionString;
        }
        public void CloseConnection() => dataProvider.CloseConnection(connection);
        public int GetSeed(String tablename)
        {
            int seed = 0;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT CAST(IDENT_CURRENT('"+tablename+"') AS INT)";
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                if (dataReader.Read())
                {
                    seed = dataReader.GetInt32(0);
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
            return seed;
        }
    }
}
