﻿using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Repository.ActiveSessionRepository;

namespace NaRegua_Api.Providers.Implementations
{
    public class SaloonProvider : ISaloonProvider
    {
        private readonly SaloonRepositorySqlServer _database;

        public SaloonProvider()
        {
            _database = new SaloonRepositorySqlServer();
        }

        public Saloon GetSaloon(string saloonCode)
        {
            var saloonResult = GetSaloonsAsync().Result.Resources.Where(x => x.SaloonCode == saloonCode).FirstOrDefault();

            return new Saloon
            {
                SaloonCode = saloonResult.SaloonCode,
                Address = saloonResult.Address,
                Name = saloonResult.Name,
                Contact = saloonResult.Contact
            };
        }

        public async Task<ListSaloonsResult> GetSaloonsAsync()
        {
            var saloons = await _database.SelectAllSaloons();

            return new ListSaloonsResult
            {
                Success = true,
                Resources = saloons is not null ? saloons.Select(x => x.ToResult()) : new List<Saloon>().Select(x => x.ToResult()),
            };
        }
    }
}
