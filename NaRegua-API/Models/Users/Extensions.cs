using static NaRegua_API.Models.Users.Requests;

namespace NaRegua_API.Models.Users
{
    public static class Extensions
    {
        public static User ToDomain(this UserRequest input)
        {
            return new User
            {
                Name = input.Name,
                Document = input.Document,
                Phone = input.Phone,
                Email = input.Email,
                Username = input.Username,
                Password = input.Password
            };
        }
    }
}
