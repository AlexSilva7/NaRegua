using NaRegua_Api.Database;
using NaRegua_Api.Models.Saloon;

namespace NaRegua_Api.Repository
{
    public abstract class SaloonRepository : SQLServerDatabase
    {
        protected abstract string SELECT_SALOON_BY_CODE { get; }
        protected abstract string SELECT_SALOONS { get; }

        public async Task<IEnumerable<Saloon>?> SelectAllSaloons()
        {
            var saloons = await ExecuteReader(
                SELECT_SALOONS,
                new Dictionary<string, object>());

            if (!saloons.Any()) return null;

            var saloonsList = new List<Saloon>();
            var count = saloons.Count;

            for (var x = 0; x < (count / 4); x++)
            {
                var saloon = new Saloon
                {
                    SaloonCode = saloons[0].ToString(),
                    Address = saloons[1].ToString(),
                    Name = saloons[2].ToString(),
                    Contact = saloons[3].ToString()
                };

                saloonsList.Add(saloon);
                saloons.RemoveRange(0, 4);
            }

            return saloonsList;
        }
    }
}
