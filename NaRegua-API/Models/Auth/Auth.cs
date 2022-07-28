using System.ComponentModel.DataAnnotations;

namespace NaRegua_API.Models.Auth
{
    public class Auth
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
