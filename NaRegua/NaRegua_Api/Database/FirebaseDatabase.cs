using Google.Cloud.Firestore;
using NaRegua_Api.Models.Users;
using Newtonsoft.Json;

namespace NaRegua_Api.Database
{
    public class FirebaseDatabase : IDatabase
    {
        private readonly string _configDb;
        private readonly string _projectId;

        public FirebaseDatabase()
        {
            _configDb = "NaReguaApiFirebase.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _configDb);

            _projectId = "react-lista-de-tarefas";
        }

        protected virtual async Task<FirestoreDb> CreateConnectionAsync()
        {
            var connection = await FirestoreDb.CreateAsync(_projectId);
            return connection;
        }

        public async Task ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            var collection = parameters?.Where(x => x.Key == "collection").First();
            var document = parameters?.Where(x => x.Key == "documentFirebase").First();
            var content = parameters?.Where(x => x.Key == "object").First();

            if(query == "insert")
            {
                using (var conn = CreateConnectionAsync())
                {
                    DocumentReference docRef =
                        conn.Result.Collection(collection?.Value.ToString()).Document(document?.Value.ToString());

                    await docRef.CreateAsync(content?.Value);
                }
            }
        }

        public async Task<List<object>> ExecuteReader(string? query, Dictionary<string, object>? parameters)
        {
            var response = new List<object>();

            var collection = parameters?.Where(x => x.Key == "collection").First();
            var document = parameters?.Where(x => x.Key == "documentFirebase").First();
            var parameter = parameters?.Where(x => x.Key != "collection" && x.Key != "documentFirebase");

            using (var conn = CreateConnectionAsync())
            {
                DocumentReference comand = 
                    conn.Result.Collection(collection?.Value.ToString()).Document(document?.Value.ToString());

                var snapshot = await comand.GetSnapshotAsync();
                var result = snapshot.ToDictionary();

                if (parameter.Any())
                {
                    foreach (var param in parameter)
                    {
                        var objs = result.Where(x => x.Key == param.Key);
                        foreach (var obj in objs)
                        {
                            var json = JsonConvert.SerializeObject(result);
                            response.Add(json);
                        }
                    }
                }
                else
                {
                    var json = JsonConvert.SerializeObject(result);
                    response.Add(json);
                }
            }

            return response;
        }
    }
}



//DocumentReference docRef = fireStoreDb.Collection("alunos").Document(id); // 1 aluno
//DocumentReference docRef = fireStoreDb.Collection("alunos").Document(id);
//DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
//if (snapshot.Exists)
//{
//    Aluno aluno = snapshot.ConvertTo<Aluno>();
//    aluno.AlunoId = snapshot.Id;
//    return aluno;
//}
//var aluno = new Aluno
//{
//    Nome = "Otto",
//    Email = "x",
//    Cidade = "x",
//    Sexo = "M"
//};
//CollectionReference colRef = _fireStoreDb.Collection("users");
//await colRef.Document("novoUser").SetAsync(user);
//await colRef.Document("novoUser").CreateAsync(user);
//await colRef.AddAsync(user);

//esse
//DocumentReference docRef = _fireStoreDb.Collection("users").Document("hahaha");
//await docRef.CreateAsync(aluno);
//ate aqui


//CollectionReference colRef = _fireStoreDb.Collection("users");
//await colRef.AddAsync(docRef);
//await colRef.AddAsync(aluno);

//Query alunoQuery = _fireStoreDb.Collection("users");
//DocumentReference alunQuery = _fireStoreDb.Collection("users").Document("hahaha");

//QuerySnapshot alunoQuerySnapshot = await alunoQuery.GetSnapshotAsync();


//var x = await alunQuery.GetSnapshotAsync();
//Dictionary<string, object> cidade = x.ToDictionary();
//string jsono = JsonConvert.SerializeObject(cidade);
//Aluno novoAluno2 = JsonConvert.DeserializeObject<Aluno>(jsono);

//var listaAluno = new List<Aluno>();
//foreach (DocumentSnapshot documentSnapshot in alunoQuerySnapshot.Documents)
//{
//    if (documentSnapshot.Exists)
//    {
//        Dictionary<string, object> city = documentSnapshot.ToDictionary();
//        string json = JsonConvert.SerializeObject(city);
//        Aluno novoAluno = JsonConvert.DeserializeObject<Aluno>(json);
//        //novoAluno.Nome = documentSnapshot.Id;
//        listaAluno.Add(novoAluno);
//    }
//}
