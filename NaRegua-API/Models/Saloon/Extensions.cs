using NaRegua_API.Common.Contracts;
using System.Linq;
using static NaRegua_API.Models.Professional.Requests;
using static NaRegua_API.Models.Saloon.Responses;

namespace NaRegua_API.Models.Saloon
{
    public static class Extensions
    {
        public static ListSaloonsResponse ToResponse(this ListSaloonsResult input)
        {
            return new ListSaloonsResponse
            {
                Resources = input.Resources.Select(item => item.ToDomain())
            };
        }

        public static SaloonsResponse ToDomain(this SaloonsResult input)
        {
            return new SaloonsResponse
            {
                SaloonCode = input.SaloonCode,
                Address = input.Address,
                Name = input.Name,
                Contact = input.Contact
            };
        }
    }
}
