using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBLL;
using Models;

namespace DBL
{
    public class CustomerDB : BaseDB<Customers>
    {
        protected override string GetTableName() => "customers"; 
        protected override string GetPrimaryKeyName() => "user_id"; 

      
        protected override async Task<Customers> CreateModelAsync(object[] row)
        {
            Customers c = new Customers
            {
                user_id = Convert.ToInt32(row[0]),
                FullName = row[1].ToString(),
                Email = row[2].ToString(),
                Password = row[3]?.ToString(),
                Role = Convert.ToInt32(row[4]),
                CreatedAt = row[5] != DBNull.Value ? Convert.ToDateTime(row[5]) : DateTime.Now
            };
            return c;
        }

        // שליפת כל המשתמשים
        public async Task<List<Customers>> GetAllAsync()
        {
            return await SelectAllAsync();
        }

        // הוספת משתמש חדש והחזרת האובייקט שנשמר
        public async Task<Customers> InsertGetObjAsync(Customers customer)
        {
            Dictionary<string, object> values = new()
            {
                { "full_name", customer.FullName },
                { "email", customer.Email },
                { "password", customer.Password },
                { "role", customer.Role },
                { "created_at", DateTime.Now }
            };

            return await base.InsertGetObjAsync(values);
        }

        // עדכון פרטי משתמש
        public async Task<int> UpdateAsync(Customers customer)
        {
            Dictionary<string, object> values = new()
            {
                { "full_name", customer.FullName },
                { "email", customer.Email },
                { "password", customer.Password },
                { "role", customer.Role }
            };

            Dictionary<string, object> filter = new()
            {
                { "user_id", customer.user_id }
            };

            return await base.UpdateAsync(values, filter);
        }

        // מחיקת משתמש לפי אובייקט
        public async Task<int> DeleteAsync(Customers customer)
        {
            Dictionary<string, object> filter = new()
            {
                { "user_id", customer.user_id }
            };

            return await base.DeleteAsync(filter);
        }

        // מחיקת משתמש לפי ID
        public async Task<int> DeleteAsync(int userId)
        {
            Dictionary<string, object> filter = new()
            {
                { "user_id", userId }
            };

            return await base.DeleteAsync(filter);
        }

        // שליפת משתמש לפי ID
        public async Task<Customers> SelectByPkAsync(int id)
        {
            Dictionary<string, object> p = new()
            {
                { "user_id", id }
            };

            List<Customers> list = await SelectAllAsync(p);
            return list.Count == 1 ? list[0] : null;
        }

        // שליפת משתמש לפי אימייל (לוגין)
        public async Task<Customers> SelectByEmailAsync(string email)
        {
            Dictionary<string, object> p = new()
            {
                { "email", email }
            };

            List<Customers> list = await SelectAllAsync(p);
            return list.Count == 1 ? list[0] : null;
        }

        // שליפת כל המשתמשים מסוג Student
        public async Task<List<Customers>> GetAllStudentsAsync()
        {
            string sql = "SELECT * FROM customers WHERE role = 'Student'";
            return await SelectAllAsync(sql);
        }

        // שליפת כל המשתמשים מסוג Teacher
        public async Task<List<Customers>> GetAllTeachersAsync()
        {
            string sql = "SELECT * FROM customers WHERE role = 'Teacher'";
            return await SelectAllAsync(sql);
        }
    }
}
