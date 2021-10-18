using System;
using System.Collections.Generic;
using CrudMongoDB.Database;
using CrudMongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CrudMongoDB.MongoDAO
{
    public sealed class BookDAO
    {
        private static BookDAO _instance;
        private static object SyncLock = new Object();

        public static BookDAO GetInstance
        {
            get {
                if (_instance == null){
                    lock (SyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BookDAO();
                            Console.WriteLine("\n\n\n:: Nova Instância Criada!");
                        }
                    }   
                }
                else
                {
                    Console.WriteLine("\n\n\n:: Reutilizando Instância!");
                }    

                return _instance;
            }
        }

        public IMongoCollection<Book> GetBooksCollectionDAO()
        {
            IMongoCollection<Book> booksCollection = MongoDBDatabase.GetConnect.GetCollection<Book>("Books");
            return booksCollection;
        }

        public void InsertBookDAO(IMongoCollection<Book> booksCollection, Book infoBook)
        {
            booksCollection.InsertOne(infoBook);
        }

        public List<Book> ListBooksDAO(IMongoCollection<Book> booksCollection)
        {
            var filter = Builders<Book>.Filter.Empty;
            var books = booksCollection.Find<Book>(filter).ToList();
            return books;
        }

        public void UpdateBookDAO(IMongoCollection<Book> booksCollection, Book infoBook, string id)
        {
            // Filtro para Buscar apenas o Livro que está sendo atualizado
            var filter = Builders<Book>.Filter.Eq(b => b.Id, ObjectId.Parse(id));

            // Populando Atualização 
            var bookUpdate = Builders<Book>.Update
                .Set(b => b.Name, infoBook.Name)
                .Set(b => b.YearPublish, infoBook.YearPublish)
                .Set(b => b.Description, infoBook.Description);

            // Efetivar Atualização do Livro
            booksCollection.UpdateOne(filter, bookUpdate);
        }

        public void DeleteBookDAO(IMongoCollection<Book> booksColletion, string id)
        {
            // Filtro para excluir apenas o Livro que contém o id passando em parâmetro
            var filter = Builders<Book>.Filter.Where(b => b.Id == ObjectId.Parse(id));

            booksColletion.DeleteOne(filter);
        }
    }
}