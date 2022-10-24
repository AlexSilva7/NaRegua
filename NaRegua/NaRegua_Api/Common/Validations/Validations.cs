using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;

namespace NaRegua_Api.Common.Validations
{
    public static class Validations
    {
        public static bool ChecksIfIsNullProperty(this object input)
        {
            var properties = input.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(input);

                    if (value == null) return true;
                    if (string.IsNullOrWhiteSpace(value.ToString())) return true;
                }
            }

            return false;
        }

        public static string FindFirstClaimOfType(this IPrincipal principal, string claimType)
        {
            if (!(principal is ClaimsPrincipal claimsPrincipal)) { return null; }

            var claim = claimsPrincipal.FindFirst(claimType);

            return claim?.Value;
        }

        public static bool IsCustomer(IPrincipal principal)
        {
            return bool.Parse(FindFirstClaimOfType(principal, "IsCustomer"));
        }

        public static bool VerifyIfIsValidCpf(this string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                cpf = cpf.Trim().Replace(".", "").Replace("-", "");
                if (cpf.Length != 11) return false;

                for (int j = 0; j < 10; j++)
                {
                    var charParse = char.Parse(j.ToString());
                    var padLeft = j.ToString().PadLeft(11, charParse);

                    if (padLeft == cpf) return false;
                }

                string tempCpf = cpf.Substring(0, 9);
                int soma = 0;

                for (int i = 0; i < 9; i++)
                {
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                }

                int resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                var digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;

                for (int i = 0; i < 10; i++)
                {
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                }

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);
            }
            catch
            {
                return false;
            }
        }

        public static bool VerifyIfIsValidEmail(this string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
