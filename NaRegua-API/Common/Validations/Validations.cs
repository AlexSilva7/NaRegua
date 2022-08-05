namespace NaRegua_API.Common.Validations
{
    public static class Validations
    {
        public static bool ChecksIfIsNullProperty(object input)
        {
            var properties = input.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(input);

                    if (value == null) return true;
                }
            }

            return false;
        }
    }
}
