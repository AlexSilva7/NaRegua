using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Models.Saloon;

namespace NaRegua_Api.Providers.Fakes
{
    public class SaloonProviderFake : ISaloonProvider
    {
        public static List<Saloon> saloons = new List<Saloon>();

        public SaloonProviderFake()
        {
            RegisterSalonsFakes("045B79", "Travessa União, nº9", "Salão Top Hair", "219785433");
            RegisterSalonsFakes("978C50", "Rua 1, nº 399", "Só os Cria", "215587499");
            RegisterSalonsFakes("022D79", "Casa da paz, nº 500", "Salão Top Hair", "219785433");
            RegisterSalonsFakes("011Z79", "Cachopa, nº 150", "Bem Bolado", "21978950");
        }

        public Saloon GetSaloon(string saloonCode)
        {
            return saloons.Find(x => x.SaloonCode == saloonCode);
        }

        public Task<ListSaloonsResult> GetSaloonsAsync()
        {
            return Task.FromResult(new ListSaloonsResult
            {
                Resources = saloons.Select(x => x.ToResult()),
                Message = "Query successfully completed!",
                Success = true
            });
        }

        private void RegisterSalonsFakes(string code, string address, string name, string contact)
        {
            saloons.Add(new Saloon
            {
                SaloonCode = code,
                Address = address,
                Name = name,
                Contact = contact,
            });
        }
    }
}
