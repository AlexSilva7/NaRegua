namespace NaRegua_API.Models.Generics
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
