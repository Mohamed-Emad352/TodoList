using System.Reflection;
using MongoDB.Driver;
using Students.API.Interfaces;
using Students.API.Models;

namespace Students.API.Data.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly IMongoCollection<Student> _studentsCollection;

    public StudentRepository()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("School");
        _studentsCollection = database.GetCollection<Student>("Students");
    }

    public async Task<List<Student>> GetAllAsync() =>
        await _studentsCollection.Find(_ => true).ToListAsync();

    public async Task<Student?> GetByIdAsync(string id) =>
        await _studentsCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Student student) =>
        await _studentsCollection.InsertOneAsync(student);

    public async Task UpdateAsync(string id, Dictionary<string, object> fields)
    {
        if (fields.Count == 0)
            throw new ArgumentException("Fields to update cannot be empty", nameof(fields));

        var filter = Builders<Student>.Filter.Eq(s => s.Id, id);
        var updates = new List<UpdateDefinition<Student>>();
        var propertyCache = new Dictionary<string, PropertyInfo>();
        
        var studentProperties = typeof(Student).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in studentProperties)
        {
            propertyCache[prop.Name] = prop;
        }

        foreach (var update in fields)
        {
            if (propertyCache.TryGetValue(update.Key, out var propertyInfo))
            {
                updates.Add(Builders<Student>.Update.Set(update.Key, update.Value));
            }
            else
            {
                throw new ArgumentException($"Invalid field name", update.Key);
            }
        }

        if (updates.Count == 0)
            throw new ArgumentException("No valid fields to update");

        var combinedUpdate = Builders<Student>.Update.Combine(updates);
        var result = await _studentsCollection.UpdateOneAsync(filter, combinedUpdate);

        if (result.MatchedCount == 0)
        {
            throw new Exception($"Student with id {id} not found.");
        }
    }

    public async Task DeleteAsync(string id) =>
        await _studentsCollection.DeleteOneAsync(s => s.Id == id);
}