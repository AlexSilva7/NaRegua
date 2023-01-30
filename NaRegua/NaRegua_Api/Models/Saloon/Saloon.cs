using Google.Cloud.Firestore;

namespace NaRegua_Api.Models.Saloon
{
    [FirestoreData]
    public class SaloonList
    {
        [FirestoreProperty]
        public List<Saloon> Saloons { get; set; }
    }

    [FirestoreData]
    public class Saloon
    {
        [FirestoreProperty]
        public string SaloonCode { get; set; }

        [FirestoreProperty]
        public string Address { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Contact { get; set; }
    }
}
