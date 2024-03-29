﻿namespace NaRegua_Api.Models.Generics
{
    public static class Extensions
    {
        public static GenericResponse ToResponse(this GenericResult input)
        {
            return new GenericResponse
            {
                Message = input.Message,
                Success = input.Success
            };
        }
    }
}
