using Google.Cloud.Firestore;
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
            var collection = parameters?.Where(x => x.Key == "collection").First().Value.ToString();
            var document = parameters?.Where(x => x.Key == "documentFirebase").First().Value.ToString();
            var content = parameters?.Where(x => x.Key == "content").First().Value;

            if (query == "insert")
            {
                using (var conn = CreateConnectionAsync())
                {
                    var docRef = conn.Result.Collection(collection).Document(document);

                    await docRef.CreateAsync(content);
                }
            }

            if(query == "update")
            {
                using (var conn = CreateConnectionAsync())
                {
                    var docRef = conn.Result.Collection(collection).Document(document);

                    await docRef.CreateAsync(content);

                    //await docRef.UpdateAsync(content);
                }
            }
        }

        public async Task<List<object>> ExecuteReader(string? query, Dictionary<string, object> parameters)
        {
            var response = new List<object>();

            var collection = parameters?.Where(x => x.Key == "collection");
            var document = parameters?.Where(x => x.Key == "documentFirebase");
            var parameter = parameters?.Where(x => x.Key != "collection" && x.Key != "documentFirebase").ToList();

            using (var conn = CreateConnectionAsync())
            {
                if (document?.FirstOrDefault().Value is not null)
                {
                    var colRef = conn.Result.Collection(collection?.First().Value.ToString());
                    var querySnapshot = await colRef.GetSnapshotAsync();

                    var docRef = 
                        conn.Result.Collection(collection?.First().Value.ToString())
                                   .Document(document.First().Value.ToString());

                    var snapshot = await docRef.GetSnapshotAsync();
                    var obj = snapshot.ToDictionary();

                    if(obj is not null)
                    {
                        response.Add(JsonConvert.SerializeObject(obj));
                    }
                }
                else
                {
                    var colRef = conn.Result.Collection(collection?.First().Value.ToString());
                    var querySnapshot = await colRef.GetSnapshotAsync();

                    foreach (var documentSnapshot in querySnapshot.Documents)
                    {
                        if (documentSnapshot.Exists)
                        {
                            var objects = documentSnapshot.ToDictionary();

                            foreach (var p in parameter)
                            {
                                if (objects.ContainsKey(p.Key))
                                {
                                    var obj = objects.Where(x => x.Key == p.Key).FirstOrDefault();

                                    if (obj.Value.ToString() == p.Value.ToString())
                                    {
                                        var json = JsonConvert.SerializeObject(objects);
                                        response.Add(json);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //var querySnapshot = await colRef.GetSnapshotAsync();

            //using (var conn = CreateConnectionAsync())
            //{
            //    if (!(document?.FirstOrDefault().Value == null))
            //    {
            //        var collectionList = collection?.ToList();
            //        var documentList = document.ToList();
            //        var comand = new object();

            //        if (collectionList?.Count != documentList.Count)
            //        {
            //            for (var x = 0; x < collectionList?.Count; x++)
            //            {
            //                if (x == 0)
            //                {
            //                    comand =
            //                        conn.Result.Collection(collectionList[x].Value.ToString()).Document(document.ToList()[0].Value.ToString());
            //                }
            //                else
            //                {
            //                    comand =
            //                        conn.Result.Collection(collectionList[x].Value.ToString());
            //                }
            //            }
            //        }

            //        //var snapshot = await comand.GetSnapshotAsync();
            //        //var result = snapshot.ToDictionary();
            //        var result = query;

            //        if(result is null)  return response;

            //        if (parameter.Any())
            //        {
            //            foreach (var param in parameter)
            //            {
            //                //var objs = result.Where(x => x.Key == param.Key);
            //                //foreach (var obj in objs)
            //                //{
            //                //    var json = JsonConvert.SerializeObject(result);
            //                //    response.Add(json);
            //                //}
            //            }
            //        }
            //        else
            //        {
            //            var json = JsonConvert.SerializeObject(result);
            //            response.Add(json);
            //        }
            //    }
            //    else
            //    {
            //        //var colRef = conn.Result.Collection(collection?.Value.ToString());
            //        //var querySnapshot = await colRef.GetSnapshotAsync();

            //        //foreach (var documentSnapshot in querySnapshot.Documents)
            //        //{
            //        //    if (documentSnapshot.Exists)
            //        //    {
            //        //        var objects = documentSnapshot.ToDictionary();

            //        //        foreach (var p in parameter)
            //        //        {
            //        //            if (objects.ContainsKey(p.Key))
            //        //            {
            //        //                var obj = objects.Where(x => x.Key == p.Key).FirstOrDefault();

            //        //                if (obj.Value.ToString() == p.Value.ToString())
            //        //                {
            //        //                    var json = JsonConvert.SerializeObject(objects);
            //        //                    response.Add(json);
            //        //                    return response;
            //        //                }
            //        //            }
            //        //        }
            //        //    }
            //        //}
            //    }
            //}

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
