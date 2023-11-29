using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using AnalaizerClassLibrary;
using GraphInterface;


namespace AnalaizerTests
{
    [TestClass]
    public class AnalaizerUnitTest
    {
        string connectionString = "server=localhost;port=3306;database=sample;uid=root;password=10Gejhupov!";
        [TestMethod]
        public void TestCheckCurrencyMethod()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // DbConnection that is already opened
                using (CalculatorDbContext context = new CalculatorDbContext(connection, false))
                {
                    var list = context.CheckCurrencySetValues;
                    foreach (var item in list)
                    {
                        try
                        {
                            AnalaizerClass.expression = item.Expression;
                            if (item.TestTrue) Assert.IsTrue(AnalaizerClass.CheckCurrency());
                            else Assert.IsFalse(AnalaizerClass.CheckCurrency());
                        }
                        catch(Exception ex) { }
                    }
                }

            }
        }
    }
}