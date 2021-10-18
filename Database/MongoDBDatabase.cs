using MongoDB.Driver;
using System;

namespace CrudMongoDB.Database
{
    public sealed class MongoDBDatabase
    {
        private static volatile MongoDBDatabase _instance;
        private static object SyncLock = new Object();
        private static IMongoDatabase db = null;

        private const string CONNECTION_STRING_CRUDMONGODB = "[SUA STRING DE CONEXÃO - PEGUE NA SUA CONTA DO MONGODB ATLAS]";
        private const string DATABASE = "[NOME DA SUA BASE DE DADOS]"; 

        private MongoDBDatabase() 
        {
            try
            {
                // Conexão Servidor
                var client = new MongoClient(CONNECTION_STRING_CRUDMONGODB);

                // Conexão / Criação Base de Dados
                db = client.GetDatabase(DATABASE); 
            }
            catch (MongoException ex)
            {
                Console.WriteLine(string.Format("ERRO AO ESTABELECE CONEXÃO COM BANCO DE DADOS: {0}", ex.Message));
            }
        }

        public static IMongoDatabase GetConnect
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MongoDBDatabase();
                            Console.WriteLine("\n\n\n:: Nova Conexão - Instância Criada!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\n\n\n:: Reutilizando Conexão!");
                }

                return db;
            }
        }
    }
}