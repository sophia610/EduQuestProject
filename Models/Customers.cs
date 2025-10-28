namespace Models
{
    public class Customers
    {
        public int user_id { get; set; }   
        public string FullName { get; set; }  
        public string Email { get; set; }     
        public string Password { get; set; }  
        public int Role { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
        //Profileimage???
        public Customers() { }

        public Customers(int user_id, string fullName, string email, string password, int role, DateTime createdAt)
        {
            this.user_id = user_id;
            this.FullName = fullName;
            this.Email = email;
            this.Password = password;
            this.Role = role;
            this.CreatedAt = createdAt;
        }
    }
}
