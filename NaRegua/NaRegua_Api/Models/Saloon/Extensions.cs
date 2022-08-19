using NaRegua_Api.Common.Contracts;
using static NaRegua_Api.Models.Saloon.Responses;

namespace NaRegua_Api.Models.Saloon
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

        public static SaloonResponse ToDomain(this SaloonResult input)
        {
            return new SaloonResponse
            {
                SaloonCode = input.SaloonCode,
                Address = input.Address,
                Name = input.Name,
                Contact = input.Contact
            };
        }

        public static SaloonResult ToResult(this Saloon input)
        {
            return new SaloonResult
            {
                SaloonCode = input.SaloonCode,
                Address = input.Address,
                Name = input.Name,
                Contact = input.Contact
            };
        }
    }
}
