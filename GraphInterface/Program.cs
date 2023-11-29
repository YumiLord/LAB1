using AnalaizerClassLibrary;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphInterface
{
    public class CheckCurrencyValues
    {
        public int Id { get; set; }
        public string Expression { get; set; }
        public bool TestTrue { get; set; } = true;
    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext()
            : base()
        {

        }
        public CalculatorDbContext(DbConnection existingConnection, bool contextOwnsConnection)
      : base(existingConnection, contextOwnsConnection)
        {

        }
        public DbSet<CheckCurrencyValues> CheckCurrencySetValues { get; set; }
    }
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string connectionString = "server=localhost;port=3306;database=sample;uid=root;password=10Gejhupov!";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // DbConnection that is already opened
                using (CalculatorDbContext context = new CalculatorDbContext(connection, false))
                {
                    
                    context.Database.CreateIfNotExists();

                    

                    var Value1 = new CheckCurrencyValues() { 
                        Expression = "((12+78))"
                    };
                    context.CheckCurrencySetValues.Add(Value1);
                    var Value2 = new CheckCurrencyValues()
                    {
                        Expression = "((345)",
                        TestTrue = false
                    };
                    context.CheckCurrencySetValues.Add(Value2);
                    var Value3 = new CheckCurrencyValues()
                    {
                        Expression = "23/6)",
                        TestTrue = false
                    };
                    context.CheckCurrencySetValues.Add(Value3);
                    var Value4 = new CheckCurrencyValues()
                    {
                        Expression = "((((78-70))))",
                        TestTrue = false
                    };
                    context.CheckCurrencySetValues.Add(Value4);

                    context.SaveChangesAsync();
                }

            }

            int argCount = args == null ? 0 : args.Length;
            if (argCount > 0)
            {
                // redirect console output to parent process;
                // must be before any calls to Console.WriteLine()
                AttachConsole(ATTACH_PARENT_PROCESS);


                AnalaizerClass.expression = args[0];

                int length = Console.CursorLeft;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(new string(' ', length));
                Console.SetCursorPosition(0, Console.CursorTop);

                Console.WriteLine("Expression:" + AnalaizerClass.expression);
                string result = AnalaizerClass.Estimate();

                ConsoleColor color = ConsoleColor.Green;

                if (result.StartsWith("&"))
                {
                    result = result.TrimStart('&');
                    color = ConsoleColor.Red;
                }
                else
                    result = result + Environment.NewLine + "Error: 0";


                ConsoleColor current = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.OutputEncoding = Encoding.UTF8;                
                Console.WriteLine("Result: " + result);                
                Console.ForegroundColor = current;

                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
