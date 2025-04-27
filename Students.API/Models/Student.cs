using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Students.API.Models;

public class Student
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Email { get; set; } = null!;
}