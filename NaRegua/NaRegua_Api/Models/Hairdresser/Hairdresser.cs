namespace NaRegua_Api.Models.Hairdresser
{
    public class Hairdresser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SaloonCode { get; set; }
        public bool IsCustomer = false;
    }
}
