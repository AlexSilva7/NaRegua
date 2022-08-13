namespace NaRegua_API.Models.Users
{
    public class User
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsCustomer = true;
    }
}
