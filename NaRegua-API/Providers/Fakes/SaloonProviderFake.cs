﻿using NaRegua_API.Common.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class SaloonProviderFake : ISaloonProvider
    {
        public static List<SaloonsResult> saloons;

        public SaloonProviderFake()
        {
            saloons = new List<SaloonsResult>();
            RegisterSalonsFakes("045B79", "Travessa União, nº9", "Salão Top Hair", "219785433");
            RegisterSalonsFakes("978C50", "Rua 1, nº 399", "Só os Cria", "215587499");
            RegisterSalonsFakes("022D79", "Casa da paz, nº 500", "Salão Top Hair", "219785433");
            RegisterSalonsFakes("011Z79", "Cachopa, nº 150", "Bem Bolado", "21978950");
        }

        public Task<SaloonsResult> GetSaloonAsync(string saloonCode)
        {
            var saloon = saloons.Find(x => x.SaloonCode == saloonCode);
            return Task.FromResult(saloon);
        }

        public Task<ListSaloonsResult> GetSaloonsAsync()
        {
            return Task.FromResult(new ListSaloonsResult
            {
                Resources = saloons,
                Message = "Query successfully completed!",
                Success = true
            });
        }

        private void RegisterSalonsFakes(string code, string address, string name, string contact)
        {
            saloons.Add(new SaloonsResult
            {
                SaloonCode = code,
                Address = address,
                Name = name,
                Contact = contact,
            });
        }
    }
}
