using Google.Cloud.Firestore;
using NaRegua_Api.Models.Users;
using Newtonsoft.Json;

namespace NaRegua_Api.Database
{
    public class FirebaseDatabase : IDatabase
    {
        private readonly FirestoreDb _fireStoreDb;
        private readonly string _configDb;
        private readonly string _projectId;

        public FirebaseDatabase()
        {
            _configDb = "react-lista-de-tarefas-firebase-adminsdk-xewbt-bfc7a92eb9.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _configDb);
            _projectId = "react-lista-de-tarefas";
            _fireStoreDb = FirestoreDb.Create(_projectId);
        }

        [FirestoreData]
        public class Aluno
        {
            //public string AlunoId { get; set; }
            [FirestoreProperty]
            public string Nome { get; set; }
            [FirestoreProperty]
            public string Email { get; set; }
            [FirestoreProperty]
            public string Cidade { get; set; }
            [FirestoreProperty]
            public string Sexo { get; set; }
        }

        public async Task ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            //DocumentReference docRef = fireStoreDb.Collection("alunos").Document(id); // 1 aluno
            //DocumentReference docRef = fireStoreDb.Collection("alunos").Document(id);
            //DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            //if (snapshot.Exists)
            //{
            //    Aluno aluno = snapshot.ConvertTo<Aluno>();
            //    aluno.AlunoId = snapshot.Id;
            //    return aluno;
            //}
            var aluno = new Aluno
            {
                Nome = "alex",
                Email = "x",
                Cidade = "x"
            };
            //CollectionReference colRef = _fireStoreDb.Collection("users");
            //await colRef.Document("novoUser").SetAsync(user);
            //await colRef.Document("novoUser").CreateAsync(user);
            //await colRef.AddAsync(user);

            CollectionReference colRef = _fireStoreDb.Collection("users");
            await colRef.AddAsync(aluno);

            Query alunoQuery = _fireStoreDb.Collection("users");
            Query alunQuery = _fireStoreDb.Collection("user");
            Query alunQuerys = _fireStoreDb.Collection("tasks");

            QuerySnapshot alunoQuerySnapshot = await alunoQuery.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in alunoQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> city = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(city);
                    //Aluno novoAluno = JsonConvert.DeserializeObject<Aluno>(json);
                    //novoAluno.AlunoId = documentSnapshot.Id;
                    //listaAluno.Add(novoAluno);
                }
            }

            throw new NotImplementedException();
        }

        public Task<List<object>> ExecuteReader(string query, Dictionary<string, object>? parameters)
        {
            throw new NotImplementedException();
        }
    }
}
