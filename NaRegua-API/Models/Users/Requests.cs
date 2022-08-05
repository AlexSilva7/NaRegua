namespace NaRegua_API.Models.Users
{
    public class Requests
    {
        public class UserRequest
        {
            public string Name { get; set; }
            public string Document { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
