using MongoDB.Bson.Serialization.Attributes;
namespace MyConsoleApp
{
    public class BookStore
    {
        [BsonId]
        public required string ISBN { get; set; }
        public required string BookTitle { get; set; }
        public required string Author { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
        [BsonIgnoreIfNull]
        public int? TotalPages { get; set; }
    }

}