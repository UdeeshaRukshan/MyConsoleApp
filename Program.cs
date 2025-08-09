using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace <USERNAME> and <PASSWORD> with your MongoDB Atlas credentials
            const string connectionUri =
                "";

            var settings = MongoClientSettings.FromConnectionString(connectionUri);

            // Set the ServerApi field of the settings object to set the version of the Stable API
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            // Create a new client and connect to the server
            var client = new MongoClient(settings);


       
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine($"✅ Pinged your deployment. You successfully connected to MongoDB! Result: {result}");

                //Create database "LibraryDB" or retrieve if exists
                var mongoDb = client.GetDatabase("LibraryDB");
                //Create collection "BookStore" or retrieve if exists
                var collection = mongoDb.GetCollection<BookStore>("BookStore");




                //Create a book list
                List<BookStore> bookStores = new List<BookStore>
{
new BookStore
{
BookTitle = "MongoDB Basics",
ISBN = "8767687689898yu",
Author = "Tanya",
Category = "NoSQL DBMS",
Price = 456
},
new BookStore
{
BookTitle = "C# Basics",
ISBN = "27758987689898yu",
Author = "Tanvi",
Category = "Programming Languages",
TotalPages = 376,
Price = 289
},
new BookStore
{
BookTitle = "SQL Server Basics",
ISBN = "117675787689898yu",
Author = "Tushar",
Category = "RDBMS",
TotalPages = 250,
Price = 478
},
new BookStore
{
BookTitle = "Entity Framework Basics",
ISBN = "6779799933389898yu",
Author = "Somya",
Category = "ORM tool",
TotalPages = 175,
Price = 289
}

};
                //Insert books to the "BookStore" collection one by one
                foreach (BookStore bookStore in bookStores)
                {
                    
                    var existingBook = collection.AsQueryable().FirstOrDefault(b => b.ISBN == bookStore.ISBN);
                    if (existingBook != null)
                    {
                        Console.WriteLine($"Book with ISBN {bookStore.ISBN} already exists. Skipping insertion.");
                        continue;
                    }

                    collection.InsertOne(bookStore);
                }
                Console.WriteLine("Books added, check your new book collection!");
                Console.ReadLine();

                //Query data
                var bookCount = collection.AsQueryable().Where(b => b.TotalPages > 200);
                Console.WriteLine("\nCount of books having more than 200 pages is => " +
                bookCount.Count());
                var book = collection.AsQueryable().Where(b => b.BookTitle.StartsWith("Mongo"));
                Console.WriteLine("\nThe book which title starts with 'Mongo' is => " +
                book.First().BookTitle);
                var cheapestBook = collection.AsQueryable().OrderBy(b => b.Price).First();
                Console.WriteLine("\nCheapest book is => " + cheapestBook.BookTitle);
                var bookWithISBN = collection.AsQueryable().Single(b => b.ISBN == "6779799933389898yu");
                Console.WriteLine("\nBook with ISBN number 6779799933389898yu is => " +
                bookWithISBN.BookTitle);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Connection failed: {ex.Message}");
            }
        }
    }
}
