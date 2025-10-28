using Models;
using DBL;

namespace ConsoleUnitTesting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await TestInsertCustomerAsync();
        }
        public static async Task TestInsertCustomerAsync()
        {
            Customers customers = new Customers();
            CustomerDB db = new CustomerDB();
            customers.FullName = "Shahar Hartshtein";
            customers.Email = "shahartt@gmail.com";
            customers.Role = 0;
            customers.Password = "shaha4";
            customers = await db.InsertGetObjAsync(customers);
            List<Customers> allCustomers = await db.GetAllAsync();
            foreach (var c in allCustomers)
            {
                Console.WriteLine($"- {c.user_id}: {c.FullName} ({c.Email}) | Role: {(c.Role == 0 ? "Student" : "Teacher")}");
            }
        }
    }
}
