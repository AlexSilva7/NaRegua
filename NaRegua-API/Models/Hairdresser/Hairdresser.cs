﻿namespace NaRegua_API.Models.Hairdresser
{
    public class Hairdresser
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string SaloonCode { get; set; }
        public bool IsCustomer = false;
    }
}
